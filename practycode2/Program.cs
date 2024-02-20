using practycode2;
using System.Text.RegularExpressions;


//var html = await Load("https://hebrewbooks.org/beis");
var html = "<html><div>   <div>        <p class=\"bla\" ></p>  </div></div></html>";
var cleanHtml = new Regex("\\s{2,}").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(x => x.Length > 0).ToList();
HtmlElement root = CreateElement(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
BuildTree(root, htmlLines.Skip(2).ToList());

var list=root.FindElements(Selector.ConvertString("div p.bla"));
foreach (var element in list)
    Console.WriteLine(element);

Console.WriteLine("-------------------------------");
Console.WriteLine("Hebrew Books Tree:");
Printing(root, "");

Console.ReadLine();
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
static HtmlElement BuildTree(HtmlElement root, List<string> lines)
{
    HtmlElement currentElement = root;
    foreach (string line in lines)
    {
        if (line.StartsWith("/html"))
            break;
        if (line.StartsWith("/"))
        {
            currentElement = currentElement.Parent;
            continue;
        }
        string firstWord = line.Split(' ')[0];
        HtmlHelper helper = HtmlHelper.Instance;

        if (! helper.AllHtmlTags.Contains(firstWord))
        {
            currentElement.InnerHtml += line;
            
            continue;
        }
        HtmlElement element = CreateElement(firstWord, currentElement, line);
        currentElement.Children.Add(element);
        currentElement = element;
        if (helper.SelfClosingHtmlTags.Contains(firstWord) ||firstWord.EndsWith("/"))
            currentElement= currentElement.Parent;

    };
    return root;
}
static HtmlElement CreateElement(string name, HtmlElement parent, string line)
{
    HtmlElement element = new HtmlElement { Name = name, Parent = parent };
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var attribute in attributes)
    {
        string nameAttribute = attribute.ToString().Split("=")[0];
        string valueAttribute = attribute.ToString().Split("=")[1].Replace("\"", "");
        
        if (nameAttribute.ToLower() == "class")
            element.Classes.Add(valueAttribute);
        else
            if (nameAttribute.ToLower() == "id")
            element.Id = valueAttribute;
        else
           if (!element.Attributes.ContainsKey(nameAttribute))
        {
            element.Attributes.Add(nameAttribute, valueAttribute);
        }
    };
    return element;

}
static void Printing(HtmlElement element, string ch)
{
    string p = ch + element.ToString();
    Console.WriteLine(p);
    foreach (HtmlElement child in element.Children)
    {
        Printing(child, ch + ' ');
    }
}