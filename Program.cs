using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Xml.Serialization;

static string GetSectionContent(IConfiguration configSection)
{
    string sectionContent = "";
    foreach (var section in configSection.GetChildren())
    {
        sectionContent += "\"" + section.Key + "\":";
        if(section.Value==null)
        {
            string subSectionContent = GetSectionContent(section);
            sectionContent += "{\n" + subSectionContent + "},\n";
        }
        else
        {
            sectionContent += "\"" + section.Value + "\",\n";
        }
    }
    return sectionContent;
}

var Configuration = new ConfigurationBuilder()
    .AddIniFile("in.ini", optional: false, reloadOnChange: true)
    .Build();

Console.WriteLine(Configuration["Position:Title_"]);

var data = new MyClass{ Field1 = "test1", Field2 = "test2" };
var serializer = new XmlSerializer(typeof(MyClass));
using (var stream = new StreamWriter("out.xml"))
    serializer.Serialize(stream, data);

Console.ReadLine();