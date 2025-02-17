namespace Accelerator.Commercetools.Importer.Shared.Interfaces;

public interface IFileConsumer<T> where T : class, new()
{
    public ValueTask ReadAsync();
    public IEnumerable<Task> StartFileConsumers();
}