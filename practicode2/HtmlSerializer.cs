using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicode2
{
    class HtmlSerializer
    {
        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        public HtmlElement Serialize(string html)
        {
            return BuildTree(SeparateHtmlTags(html));
        }
        private IEnumerable SeparateHtmlTags(string html)
        {
            //var html = await Load("https://hebrewbooks.org");
            //var html = "<div id=\"header\">\r\nheader\r\n<a href=\"#\">products\r\n<ul class=\"list\">\r\n<li>new collection</li>\r\n<li>sale</li>\r\n<li>for club members</li>\r\n</ul>\r\n</a>\r\n<a href=\"#\">about us\r\n<ul class=\"list\">\r\n<li>our experience</li>\r\n<li>our custemers</li>\r\n<li>recomandations</li>\r\n</ul>\r\n</a>\r\n\r\n<a href=\"#\">contact us\r\n<ul class=\"list\">\r\n<li>call us</li>\r\n<li>write us</li>\r\n<li>leave a massage</li>\r\n</ul>\r\n</a>\r\n</div>";
            var cleanHtml = new Regex("[\\t\\n\\r\\v\\f]").Replace(html, "");
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
            return htmlLines;
        }

        private HtmlElement BuildTree(IEnumerable htmlLines)
        {
            HtmlElement rootElement = new HtmlElement() { Name = "html" };
            var current = rootElement;
            foreach (string line in htmlLines)
            {
                string firstWord = line.Split(' ')[0];
                if (firstWord.Equals("/html"))
                    return rootElement;
                else if (firstWord.StartsWith('/'))
                    current = current.Parent != null ? current.Parent : current;
                else if (HtmlHelper.Helper.HtmlTags.Contains(firstWord) || HtmlHelper.Helper.HtmlVoidTags.Contains(firstWord))
                {
                    HtmlElement newElement = new HtmlElement() { Name = firstWord, Parent = current };
                    current.Children.Add(newElement);
                    string shortLine = line.Substring(firstWord.Length);
                    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(shortLine).ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);
                    newElement.Attributes = attributes;
                    foreach (var attribute in attributes)
                    {
                        if (attribute.Key == "class")
                        {
                            var classes = attribute.Value.Split(' ');
                            newElement.Classes = classes.ToList();
                        }
                        else if (attribute.Key == "id")
                            newElement.Id = attribute.Value;
                    }
                    if (!(shortLine.EndsWith('/') || HtmlHelper.Helper.HtmlVoidTags.Contains(firstWord)))
                        current = newElement;
                }
                else
                {
                    current.InnerHtml = line;
                }
            }
            return rootElement;
        }
        private void PrintTreeHtmlElement(HtmlElement root)
        {
            if (root == null)
                return;
            Console.WriteLine(root.ToString());
            for (int i = 0; i < root.Children.Count; i++) { PrintTreeHtmlElement(root.Children[i]); }
        }





    }
}
