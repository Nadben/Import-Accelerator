using System.Xml.Linq;
using Accelerator.Schema.Generator.Parsers.Interface;
using Accelerator.Schema.Generator.Parsers.Model;

namespace Accelerator.Schema.Generator.Parsers;

public class XmlSchemaParser : ISchemaParser
{
    public bool CanHandle(string fileExtension) => fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase);

    public ClassSchema Parse(string fileContent, string fileName, string folderName)
    {
        var schema = new ClassSchema { ClassName = folderName };
        var xml = XElement.Parse(fileContent);
        var firstElement = xml.Elements().FirstOrDefault();

        if (firstElement != null)
        {
            foreach (var element in firstElement.Elements())
            {
                schema.Properties[element.Name.LocalName] = typeof(string);
            }
        }

        return schema;
    }
}