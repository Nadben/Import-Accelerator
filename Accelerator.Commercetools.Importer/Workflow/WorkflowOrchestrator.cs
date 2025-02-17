using Accelerator.Commercetools.Importer.GenericEtlPipeline;
using Accelerator.Shared.Infrastructure.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.Workflow;

public class WorkflowOrchestrator : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly TaskQueue _taskQueue;
    private readonly ILogger<WorkflowOrchestrator> _logger;

    public WorkflowOrchestrator(
        IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime,
        TaskQueue taskQueue,
        ILogger<WorkflowOrchestrator> logger)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
        _taskQueue = taskQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WorkflowOrchestrator started");
        await EnqueueWorkflowTasksAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var task = await _taskQueue.DequeueAsync(stoppingToken);
                await task(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Workflow execution canceled");
                _hostApplicationLifetime.StopApplication();
                break;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred: {ExMessage}", ex.Message);
                _hostApplicationLifetime.StopApplication();
            }
        }

        _logger.LogInformation("WorkflowOrchestrator stopped");
        _logger.LogInformation("Workflow Complete");
        _hostApplicationLifetime.StopApplication();
    }
    
    private async Task EnqueueWorkflowTasksAsync(CancellationToken cancellationToken)
    {
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(i => i.FullName.Contains("Accelerator.Shared.Infrastructure"))?
            .GetTypes();

        if (allTypes is null)
        {
            throw new ArgumentNullException();
        }

        var typeGroupDictionary = new GroupedTypeDictionary()
            .RegisterGroup("Landing", type => type.Namespace?.Contains("Accelerator.Shared.Infrastructure.Entities.Landing.Generated") ?? false,
                allTypes,
                ["Price", "Category"])
            .RegisterGroup("Staging", type => type.Namespace?.Contains("Accelerator.Shared.Infrastructure.Entities.Staging") ?? false,
                allTypes,
                ["Price", "Category"]);
        
        var workerGroups = typeGroupDictionary.Categories
            .Select(category => WorkerGroup.Create(
                category,
                createLandingWorker: (landing, _) => typeof(LandingWorker<,>).MakeGenericType(landing.Type, typeof(LandingImportContext)),
                createStagingWorker: (landing, staging) => typeof(StagingWorker<,,,>).MakeGenericType(landing.Type, typeof(LandingImportContext), staging.Type, typeof(StagingImportContext)),
                landingTypes: typeGroupDictionary.GetTypes("Landing", category),
                stagingTypes: typeGroupDictionary.GetTypes("Staging", category)
            ))
            .ToList();
        
        foreach (var workerGroup in workerGroups.OrderBy(g => g.Category))
        {
            _logger.LogInformation("Processing category group: {Category}", workerGroup.Category);

            foreach (var workerType in workerGroup.WorkerTypes)
            {
                _logger.LogInformation("Enqueuing task for worker: {WorkerTypeName}", workerType.Name);

                await _taskQueue.EnqueueAsync(async ct =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var workerInstance = scope.ServiceProvider.GetRequiredService(workerType);

                    var executeMethod = workerType.GetMethod("ExecuteAsync");
                    if (executeMethod != null)
                    {
                        await (Task)executeMethod.Invoke(workerInstance, [ct])!;
                    }
                }, cancellationToken);
            }
        }
    }
}