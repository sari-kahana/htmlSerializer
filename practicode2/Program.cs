using practicode2;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

string s = "<div id=\"a\" \nclass=\"as av an\" src=\"#\">      <p>Hello World</p>    <a href=\"#\"       >Link</a></div>";
var htmlSerializer = new HtmlSerializer();
var html = await htmlSerializer.Load("https://hebrewbooks.org/beis");
var dom = htmlSerializer.Serialize(html);

var result = dom.FindElements(Selector.QuerySelector("div.popup div#popupForm.formPopup a"));
foreach (var item in result)
    Console.WriteLine(item);
