namespace Accelerator.Schema.Generator.Parsers.Model;

public record ClassSchema
{
    public string ClassName { get; set; } = string.Empty;
    public Dictionary<string, Type> Properties { get; set; } = new();
}