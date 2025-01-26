using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicode2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                foreach(var child in q.First().Children)
                    q.Enqueue(child);
                yield return q.Dequeue();
            }
        }
        public IEnumerable Ancestors()
        {
            HtmlElement current = this;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public HashSet<HtmlElement> FindElements(Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            FindElementsRec(selector,this, results);
            return results;
        }
        public void FindElementsRec(Selector selector, HtmlElement current, HashSet<HtmlElement> set)
        {
            if(selector==null)
                return;

            IEnumerable children = current.Descendants();

            foreach (HtmlElement child in children)
            {
                if (MatchesSelector(child, selector))
                {
                    if(selector.Child == null)
                        set.Add(child);
                }
            }
            FindElementsRec(selector.Child, current, set);

        }
        private bool MatchesSelector(HtmlElement element, Selector selector)
        {
            //var s = "\"" + selector.Id + "\"";
            if (selector.Id != null &&!selector.Id.Equals(element.Id))
                return false;
            if (selector.TagName != element.Name)
                return false;

            foreach (var c in selector.Classes)
                if (element.Classes.Count > 0 && !element.Classes.Contains(c))
                    return false;
            return true;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Id: {Id}");
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine("Attributes:");
            if (Attributes!=null)
            {
                foreach (var attribute in Attributes)
                {
                    stringBuilder.AppendLine($"   {attribute.Key}: {attribute.Value}");
                }
            }
            
            stringBuilder.AppendLine("Classes:");
            foreach (var className in Classes)
            {
                stringBuilder.AppendLine($"   {className}");
            }
            stringBuilder.AppendLine($"InnerHtml: {InnerHtml}");
            stringBuilder.AppendLine($"Parent: {Parent?.Id ?? Parent?.Name ?? "null"}"); // Parent might be null
            stringBuilder.AppendLine("Children:");
            foreach (var child in Children)
            {
                stringBuilder.AppendLine($"   {child.Name ?? child.Id}");
            }

            return stringBuilder.ToString();
        }
    }
}
