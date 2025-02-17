using System.Transactions;
using Accelerator.Commercetools.Importer.Shared.Channels;
using Accelerator.Commercetools.Importer.Shared.Extension;
using Accelerator.Commercetools.Importer.Shared.Helper;
using Accelerator.Commercetools.Importer.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.Shared.Services;

public class DataBaseService<T, TContext> : IDataBaseService<T, TContext>
    where T : class, new()
    where TContext : DbContext, new()
{
    private readonly ILogger<DataBaseService<T, TContext>> _logger;
    private readonly IDbContextFactory<TContext> _contextFactory;
    private readonly DataBaseProcessingChannel<T> _dataBaseProcessingChannel;

    public DataBaseService(ILogger<DataBaseService<T, TContext>> logger,
        IDbContextFactory<TContext> contextFactory,
        DataBaseProcessingChannel<T> dataBaseProcessingChannel)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _dataBaseProcessingChannel = dataBaseProcessingChannel;
    }

    public IEnumerable<Task> StartConsumersForDatabase()
    {
        var maxParallelism = Math.Min(Environment.ProcessorCount - 2, 6);
        var tasks = new List<Task>();
        for (var i = 0; i < maxParallelism; i++)
        {
            tasks.Add(Task.Run(SaveToDatabaseAsync));
        }
        return tasks;
    }

    public async Task SaveToDatabaseAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        await foreach (var entitiesToInsert in _dataBaseProcessingChannel.ProcessingChannel.Reader.ReadAllBatches(100_000))
        {
            _logger.LogInformation("Batch start Size -> {0}", entitiesToInsert.Length);
            await BatchInsertAsync(entitiesToInsert, context);
        }
    }
    
    public async Task BatchInsertAsync(IEnumerable<T> entitiesToInsert, TContext context)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        context.ChangeTracker.AutoDetectChangesEnabled = false;
        
        try
        {
            await context.Set<T>().AddRangeAsync(entitiesToInsert);
            await context.SaveChangesAsync();
            _logger.LogInformation("Batch successfully inserted");
        }
        catch (Exception ex)
        {
           _logger.LogError("Error inserting batch: {ExMessage} {Newline}Inner Exception: {InnerException} ",
               ex.Message,
               Environment.NewLine,
               ex.InnerException?.Message);
        }
    
        scope.Complete();
    }
    
    #region Old

    public IEnumerable<Task> StartConsumersForDatabaseOld()
    {
        var maxParallelism = Math.Min(Environment.ProcessorCount - 2, 6);
        var tasks = new List<Task>();
        for (var i = 0; i < maxParallelism; i++)
        {
            tasks.Add(Task.Run(SaveToDatabaseOld));
        }

        return tasks;
    }

    private async Task SaveToDatabaseOld()
    {
        await foreach (var entitiesToInsert in _dataBaseProcessingChannel.ProcessingChannel.Reader.ReadAllBatches(100_000))
        {
            Console.WriteLine("Batch start Size -> {0}", entitiesToInsert.Length);

            using (TransactionScope scope = new TransactionScope())
            {
                TContext context = null;
                try
                {
                    context = _contextFactory.CreateDbContext();
                    context.ChangeTracker.AutoDetectChangesEnabled = false;

                    int count = 0;
                    foreach (var entityToInsert in entitiesToInsert)
                    {
                        ++count;
                        context = AddToContextOld(context, entityToInsert, count, 1000, true);
                    }

                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

                scope.Complete();
            }
        }
    }

    public TContext AddToContextOld(TContext context, T entity, int count, int commitCount, bool recreateContext)
    {
        context.Set<T>().Add(entity);

        if (count % commitCount == 0)
        {
            context.SaveChanges();
            context.ChangeTracker.Clear();
            if (recreateContext)
            {
                context.Dispose();
                context = _contextFactory.CreateDbContext();
                context.ChangeTracker.AutoDetectChangesEnabled = false;
            }
        }

        return context;
    }

    #endregion

    #region Npgs COPY with Helper

    public IEnumerable<Task> StartConsumersForDatabaseBulkCopy()
    {
        var maxParallelism = Math.Min(Environment.ProcessorCount - 2, 6);
        var tasks = new List<Task>();
        for (var i = 0; i < maxParallelism; i++)
        {
            tasks.Add(Task.Run(BulkCopy));
        }

        return tasks;
    }

    private void BulkCopy()
    {
        var sqlCopyHelper = SqlCopyHelper.CreateHelper<T>("dbo", nameof(T));
        // undertakingHelper.SaveAll(transaction.UnderlyingTransaction.Connection as Npgsql.NpgsqlConnection, undertakingsToAdd));
    }

    #endregion
}