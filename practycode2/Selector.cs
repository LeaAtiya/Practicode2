using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practycode2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
       

        public static Selector ConvertString(string str)
        {
            string[] selectors = str.Split(' ');
            Selector root=new Selector();
            Selector current=root;
            foreach (string selector in selectors)
            {
                string[] parts = new Regex("(?=[#\\.])").Split(selector).Where(p => p.Length > 0).ToArray();
                foreach (string part in parts)
                {
                    if(part.StartsWith("#"))
                        current.Id = part.Substring(1);
                    else
                        if(part.StartsWith("."))
                        current.Classes.Add(part.Substring(1));
                    else
                    {
                        HtmlHelper helper= HtmlHelper.Instance;
                        if(helper.AllHtmlTags.Contains(part))
                            current.TagName = part;
                        else
                            Console.WriteLine("you send me an invalid tag name: "+part);
                    }
                }
                Selector childSelector = new Selector();
                childSelector.Parent = current;
                current.Child = childSelector;
                current=current.Child;
            }
            current.Parent.Child = null;
            return root;
        }
    }
}
