namespace Accelerator.Commercetools.Importer.Workflow;

public class WorkerGroup
{
    public string Category { get; init; }
    public List<Type> WorkerTypes { get; init; }

    public WorkerGroup(string category, List<Type> workerTypes)
    {
        Category = category;
        WorkerTypes = workerTypes;
    }

    public static WorkerGroup Create(string category,
        Func<GroupedType, GroupedType, Type> createLandingWorker,
        Func<GroupedType, GroupedType, Type> createStagingWorker,
        IEnumerable<GroupedType> landingTypes,
        IEnumerable<GroupedType> stagingTypes)
    {
        var workerTypes = new List<Type>();

        foreach (var landingType in landingTypes)
        {
            foreach (var stagingType in stagingTypes)
            {
                workerTypes.Add(createLandingWorker(landingType, stagingType));
                workerTypes.Add(createStagingWorker(landingType, stagingType));
            }
        }

        return new WorkerGroup(category, workerTypes);
    }
}