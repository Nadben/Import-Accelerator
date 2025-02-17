using System.Text.Json;
using Accelerator.Schema.Generator.Parsers.Interface;
using Accelerator.Schema.Generator.Parsers.Model;

namespace Accelerator.Schema.Generator.Parsers;

public class JsonSchemaParser : ISchemaParser
{
    public bool CanHandle(string fileExtension) => fileExtension.Equals(".json", StringComparison.OrdinalIgnoreCase);

    public ClassSchema Parse(string fileContent, string fileName, string folderName)
    {
        var schema = new ClassSchema { ClassName = folderName };
        var json = JsonDocument.Parse(fileContent).RootElement;

        if (json.ValueKind == JsonValueKind.Array && json.GetArrayLength() > 0)
        {
            var firstObject = json[0];
            if (firstObject.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in firstObject.EnumerateObject())
                {
                    schema.Properties[property.Name] = InferType(property.Value);
                }
            }
        }

        return schema;
    }

    private Type InferType(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.Number => value.TryGetInt32(out _) ? typeof(int) : typeof(double),
        JsonValueKind.String => DateTime.TryParse(value.GetString(), out _) ? typeof(DateTime) : typeof(string),
        JsonValueKind.True or JsonValueKind.False => typeof(bool),
        _ => typeof(object)
    };
}