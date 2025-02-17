using Accelerator.Commercetools.Importer.Shared.Consumer;
using Accelerator.Commercetools.Importer.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

[WorkflowTask(1)]
public class LandingWorker<TEntity, TContext> 
    where TEntity : class, new()
    where TContext : DbContext 
{
    private readonly ILogger<LandingWorker<TEntity,TContext>> _logger;
    private readonly LandingDataConsumer<TEntity, TContext> _landingDataConsumer;

    public LandingWorker(
        ILogger<LandingWorker<TEntity,TContext>> logger, 
        LandingDataConsumer<TEntity, TContext> landingDataConsumer
        )
    {
        _logger = logger;
        _landingDataConsumer = landingDataConsumer;
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Landing worker running for {EntityType}", typeof(TEntity).Name);
            await _landingDataConsumer.TruncateTable();
            await _landingDataConsumer.DoWorkAsync();
        }   
        catch (Exception e)
        {
            _logger.LogError(e, "Error during landing stage for {EntityType}, cancelling task", typeof(TEntity).Name);
        }
    }
}