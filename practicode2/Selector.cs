using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicode2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }
        public static Selector QuerySelector(string query)
        {
            var words = query.Split(' ');
            Selector rootSelector = new Selector();
            Selector current = rootSelector;
            foreach (string word in words)
            {
                string part = word;
                int idOrClass = part.IndexOfAny(new[] { '#', '.' });
                if (idOrClass == -1)
                    current.TagName = part;
                else
                {
                    current.TagName = part.Substring(0, idOrClass);
                    part = part.Substring(idOrClass);
                }

                int idIndex = part.IndexOf('#'), idEndIndex = part.IndexOf('.') == -1 ? part.Length : part.IndexOf('.');
                if (idIndex != -1)
                {
                    current.Id = part.Substring(idIndex + 1, idEndIndex - idIndex - 1);
                    part = part.Remove(idIndex + 1, idEndIndex - idIndex - 1);
                }

                if (part.Length > 0)
                    current.Classes = part.Split('.').ToList();

                Selector newSelector = new Selector();
                current.Child = newSelector;
                current.Child.Parent = current;
                current = newSelector;
            }
            return rootSelector;
        }
    }
}
