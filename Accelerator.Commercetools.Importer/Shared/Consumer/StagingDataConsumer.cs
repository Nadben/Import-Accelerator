using System.Diagnostics;
using Accelerator.Commercetools.Importer.Shared.Channels;
using Accelerator.Commercetools.Importer.Shared.Interfaces;
using Accelerator.Shared.Infrastructure.Entities.Staging;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.Shared.Consumer;

public class StagingDataConsumer<T, TContext, TU, TUContext> : IStagingDataService<T,TContext, TU, TUContext>, IService
        where T : class, new()
        where TU : class, new()
        where TContext : DbContext, new()
        where TUContext : DbContext, new()
{
    private readonly TUContext _stagingContext;
    private readonly TContext _landingContext;
    private readonly IMapper _mapper;
    private readonly ILogger<StagingDataConsumer<T, TContext, TU, TUContext>> _logger;
    private readonly DataBaseProcessingChannel<TU> _dataBaseProcessingChannel;
    private readonly IDataBaseService<TU, TUContext> _dataBaseService;

    public StagingDataConsumer(TUContext stagingContext,
        TContext landingContext,
        IMapper mapper,
        IDataBaseService<TU, TUContext> dataBaseService, 
        DataBaseProcessingChannel<TU> dataBaseProcessingChannel, 
        ILogger<StagingDataConsumer<T, TContext, TU, TUContext>> logger)
    {
        _stagingContext = stagingContext;
        _landingContext = landingContext;
        _mapper = mapper;
        _dataBaseService = dataBaseService;
        _dataBaseProcessingChannel = dataBaseProcessingChannel;
        _logger = logger;
    }

    public async Task DoWorkAsync()
    {
        var watch = Stopwatch.StartNew();
        var savingTasks = _dataBaseService.StartConsumersForDatabase().ToList();

        await ResetHashCodeForTable();
        
        _logger.LogInformation("transforming entities of type : {Type}", typeof(T).Name);
        var mappedEntities = Transform().ToList();

        _logger.LogInformation("setting previous hash to hash");
        mappedEntities
            .OfType<TransformBase>()
            .ToList()
            .ForEach(entity => entity.PreviousHash = entity.Hash);
        
        foreach (var mappedEntity in mappedEntities)
        {
            await _dataBaseProcessingChannel.ProcessingChannel.Writer.WriteAsync(mappedEntity);
        }
        
        _dataBaseProcessingChannel.ProcessingChannel.Writer.Complete();
        
        await Task.WhenAll(savingTasks);
        watch.Stop();
        _logger.LogInformation("It took {Ms}ms", watch.ElapsedMilliseconds);
    }
    public ParallelQuery<TU> Transform() => _landingContext.Set<T>().AsParallel().Select(i => _mapper.Map<TU>(i));

    public async Task ResetHashCodeForTable()
    {
        var query = _stagingContext.Set<TU>()
            .OfType<TransformBase>()
            .Where(i => i.Hash != "0" && i.Hash != i.PreviousHash);

        if (!await query.AnyAsync())
        {
            return;
        }

        var transformBases = await query.ToListAsync();

        foreach (var transformBase in transformBases)
        {
            transformBase.Hash = "0";
        }

        await _stagingContext.SaveChangesAsync();
    }
}