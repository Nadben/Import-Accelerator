using Accelerator.Commercetools.Importer.Shared.Consumer;
using Accelerator.Commercetools.Importer.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

[WorkflowTask(2)]
public sealed class StagingWorker<TEntity, TContext, TUEntity, TUContext> 
    where TEntity : class, new()
    where TUEntity : class, new()
    where TContext : DbContext, new()
    where TUContext : DbContext, new()
{
    private readonly ILogger<StagingWorker<TEntity, TContext, TUEntity, TUContext>> _logger;
    private readonly StagingDataConsumer<TEntity, TContext, TUEntity, TUContext> _stagingDataConsumer;

    public StagingWorker(ILogger<StagingWorker<TEntity, TContext, TUEntity, TUContext>> logger,
        StagingDataConsumer<TEntity, TContext, TUEntity, TUContext> stagingDataConsumer)
    {
        _logger = logger;
        _stagingDataConsumer = stagingDataConsumer;
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Staging worker running for {EntityType}", typeof(TEntity).Name);
            await _stagingDataConsumer.DoWorkAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error in Staging Worker {EntityType}: {Message} \r\nInner Exception: {InnerException}", 
                typeof(TEntity).Name,
                e.Message,
                e.InnerException?.Message);
            throw;
        }
    }
}