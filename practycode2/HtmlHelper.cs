using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text.Json;


namespace practycode2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllHtmlTags { get; private set; }
        public string[] SelfClosingHtmlTags { get; private set; }

        private HtmlHelper()
        {
            var pathAll = File.ReadAllText("seed/AllHtmlTags.json");
            var pathSelf = File.ReadAllText("seed/SelfClosingHtmlTags.json");
            
            AllHtmlTags = JsonSerializer.Deserialize<string[]>(pathAll);
            SelfClosingHtmlTags = JsonSerializer.Deserialize<string[]>(pathSelf);
        }
       

    }

}
