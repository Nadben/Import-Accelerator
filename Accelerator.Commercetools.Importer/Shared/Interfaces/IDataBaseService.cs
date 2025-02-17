using Microsoft.EntityFrameworkCore;

namespace Accelerator.Commercetools.Importer.Shared.Interfaces;

public interface IDataBaseService<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    public IEnumerable<Task> StartConsumersForDatabase();
    public Task SaveToDatabaseAsync();
    public Task BatchInsertAsync(IEnumerable<TEntity> entity, TContext context);
    public IEnumerable<Task> StartConsumersForDatabaseOld();
    public IEnumerable<Task> StartConsumersForDatabaseBulkCopy();

}