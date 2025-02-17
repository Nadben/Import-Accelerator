namespace Accelerator.Commercetools.Importer.Workflow;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class WorkflowTaskAttribute : Attribute
{
    public int Order { get; }

    public WorkflowTaskAttribute(int order = 0)
    {
        Order = order;
    }
}