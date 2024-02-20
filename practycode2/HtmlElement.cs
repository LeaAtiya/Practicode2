using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practycode2
{
    internal class HtmlElement
    {
        private string id;
        private string name;
        private Dictionary<string, string> attributes = new Dictionary<string, string>();
        private List<string> classes = new List<string>();
        private string innerHtml = " ";
        private HtmlElement parent;
        private List<HtmlElement> children = new List<HtmlElement>();

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public List<string> Classes { get => classes; set => classes = value; }
        public Dictionary<string, string> Attributes { get => attributes; set => attributes = value; }
        public string InnerHtml { get => innerHtml; set => innerHtml = value; }
        internal HtmlElement Parent { get => parent; set => parent = value; }
        internal List<HtmlElement> Children { get => children; set => children = value; }

        public override string ToString()
        {
            string s = "";
            if (name != null)
                s += "Name: " + name + " ";
            if (id != null)
                s += "Id: " + id + " ";
            if (classes.Count > 0)
            {
                s += "Classes: ";
                foreach (var c in classes)
                {
                    s += c + " ";
                };
            }
            return s;

        }

        //Descendants && Ancestors 
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }

        //Find selector in HTML
        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            foreach (var child in Descendants())
            {
                child.FindElements(selector, results);
            }
            return results;
        }

        private void FindElements(Selector selector, HashSet<HtmlElement> results)
        {
            if (!Matches(selector))
                return;

            if (selector.Child == null)
                results.Add(this);
            else
            {
                int count = 0;
                foreach (var child in this.Descendants())
                {
                    count++;
                    if (count > 1)
                        child.FindElements(selector.Child, results);
                }
            }
        }

        private bool Matches(Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && selector.TagName != this.Name)
                return false;

            if (!string.IsNullOrEmpty(selector.Id) && selector.Id != this.Id)
                return false;

            if (selector.Classes != null && selector.Classes.Any() && !selector.Classes.All(c => this.Classes.Contains(c)))
                return false;

            return true;
        }
    }
}


