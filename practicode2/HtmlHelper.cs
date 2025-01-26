using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace practicode2
{
    internal class HtmlHelper
    {
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private readonly static HtmlHelper _helper = new HtmlHelper();
        public static HtmlHelper Helper => _helper;

        private HtmlHelper() 
        {
            HtmlTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("JSON Files/htmlTags.json"));
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("JSON Files/htmlVoidTags.json"));
        }
    }
}
