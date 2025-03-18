using Accelerator.Commercetools.Importer.Commercetools;
using Accelerator.Commercetools.Importer.Workflow;
using Accelerator.Shared.Infrastructure.Entities.Staging;
using commercetools.Sdk.ImportApi.Models.Common;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

[WorkflowTask(3)]
public class LoadingWorker<T, TContext, TURequest>
    where T : class
    where TContext : DbContext 
    where TURequest : IImportResource
{
    private readonly ILogger<LoadingWorker<T, TContext, TURequest>> _logger;
    private readonly IImportApi _importApi;
    private readonly TContext _context;
    private readonly IMapper _mapper;
    
    public LoadingWorker(ILogger<LoadingWorker<T, TContext, TURequest>> logger, 
        IImportApi importApi, 
        TContext context,
        IMapper mapper)
    {
        _logger = logger;
        _importApi = importApi;
        _context = context;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting loading worker...");
        try
        {
            // Fetch from context all obj where hash != previousHash
            var query = _context.Set<T>()
                .OfType<TransformBase>()
                .Where(i => i.Hash != i.PreviousHash)
                .Cast<T>();

            if (!await query.AnyAsync(stoppingToken))
            {
                return;
            }
            
            // map T to TURequest with mapster
            var requests = query.AsParallel()
                .Select(i => _mapper.Map<TURequest>(i))
                .ToList();
            
            // send over the wire to import api
            await _importApi.BatchInsert(requests);
            
            _logger.LogInformation("setting previous hash to hash");
            
            query.OfType<TransformBase>()
                .ToList()
                .ForEach(entity => entity.PreviousHash = entity.Hash);
            
            _context.Set<T>().UpdateRange(query);
        }   
        catch (Exception e)
        {
            _logger.LogError("Error in loading worder {EntityType}: {Message} \r\n Inner Exceptio: {InnerException}",
                typeof(T).Name, e.Message, e.InnerException?.Message);
            throw;
        }
    }
}