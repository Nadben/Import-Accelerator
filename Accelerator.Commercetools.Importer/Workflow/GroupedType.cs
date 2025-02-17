namespace Accelerator.Commercetools.Importer.Workflow;

public class GroupedType
{
    public string Group { get; }
    public string Category { get; }
    public Type Type { get; }

    public GroupedType(string group, string category, Type type)
    {
        Group = group;
        Category = category;
        Type = type;
    }
}
