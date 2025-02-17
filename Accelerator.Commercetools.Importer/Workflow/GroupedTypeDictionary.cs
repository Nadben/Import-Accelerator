namespace Accelerator.Commercetools.Importer.Workflow;

public class GroupedTypeDictionary
{
    private readonly Dictionary<string, List<GroupedType>> _groupedTypes = new();

    public GroupedTypeDictionary RegisterGroup(
        string group, 
        Func<Type, bool> predicate, 
        IEnumerable<Type> allTypes,
        string[] categories
        )
    {
        var types = allTypes.Where(predicate)
            .SelectMany(type =>
                categories.Where(category => type.Name.Contains(category))
                    .Select(category => new GroupedType(group, category, type)));

        if (!_groupedTypes.ContainsKey(group))
            _groupedTypes[group] = new List<GroupedType>();

        _groupedTypes[group].AddRange(types);
        return this;
    }

    public IEnumerable<GroupedType> GetTypes(string group, string category)
        => _groupedTypes.TryGetValue(group, out var types)
            ? types.Where(t => t.Category == category)
            : Enumerable.Empty<GroupedType>();

    public IEnumerable<string> Categories => _groupedTypes
        .SelectMany(kv => kv.Value.Select(gt => gt.Category))
        .Distinct();

    public IEnumerable<string> Groups => _groupedTypes.Keys;

    public List<GroupedType> this[string group] => _groupedTypes.TryGetValue(group, out var types) ? types : new List<GroupedType>();
}
