using Accelerator.Schema.Generator.Parsers.Interface;
using Accelerator.Schema.Generator.Parsers.Model;

namespace Accelerator.Schema.Generator.Parsers;

public class SchemaParserManager
{
    private readonly List<ISchemaParser> _parsers;

    public SchemaParserManager()
    {
        _parsers = new List<ISchemaParser>
        {
            new JsonSchemaParser(),
            new CsvSchemaParser(),
            new XmlSchemaParser(),
            new TextSchemaParser()
        };
    }

    public ClassSchema ParseFile(string fileContent, string fileExtension, string fileName, string folderName)
    {
        var parser = _parsers.FirstOrDefault(p => p.CanHandle(fileExtension));
        if (parser == null)
        {
            throw new NotSupportedException($"File type '{fileExtension}' is not supported.");
        }

        return parser.Parse(fileContent, fileName, folderName);
    }
}