using System.Globalization;
using Accelerator.Schema.Generator.Parsers.Interface;
using Accelerator.Schema.Generator.Parsers.Model;

namespace Accelerator.Schema.Generator.Parsers;

public class TextSchemaParser : ISchemaParser
{
    public bool CanHandle(string fileExtension) => fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase);

    public ClassSchema Parse(string fileContent, string className, string folderName)
    {
        var lines = fileContent.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0) throw new InvalidOperationException("The text file is empty.");

        var headers = lines[0].Split(Separator.TabSeparator);
        var firstDataRow = lines.Length > 1 ? lines[1].Split('\t') : null;

        var properties = new Dictionary<string, Type>();
        var textInfo = new CultureInfo("en-us", false).TextInfo;
        
        for (int i = 0; i < headers.Length; i++)
        {
            var columnName = textInfo.ToTitleCase(headers[i].Trim())
                .Replace("-", string.Empty)
                .Replace("_", string.Empty)
                .Replace(" ", string.Empty);
            
            var columnType = InferColumnType(firstDataRow?[i]);

            properties.Add(columnName, columnType);
        }

        return new ClassSchema
        {
            ClassName = $"{folderName}_{className}",
            Properties = properties
        };
    }

    private Type InferColumnType(string? value)
    {
        if (string.IsNullOrEmpty(value)) return typeof(string);
        if (int.TryParse(value, out _)) return typeof(int);
        if (double.TryParse(value, out _)) return typeof(double);
        if (DateTime.TryParse(value, out _)) return typeof(DateTime);

        return typeof(string);
    }
}