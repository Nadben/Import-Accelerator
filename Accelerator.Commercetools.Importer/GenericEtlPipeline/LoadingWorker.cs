using System.Collections;
using Accelerator.Commercetools.Importer.Commercetools;
using Accelerator.Commercetools.Importer.Workflow;
using Accelerator.Shared.Infrastructure.Entities.Staging;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

[WorkflowTask(3)]
public class LoadingWorker<T, TContext, TURequest>
    where T : class
    where TContext : DbContext 
    where TURequest : class, new()
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
            await _importApi.BatchInsert(requests, "container");
        }   
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

public interface ILoadingWorker<T, T1, T2>;
