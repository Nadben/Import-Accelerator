using System.Diagnostics;
using System.Transactions;
using Accelerator.Commercetools.Importer.Shared.Channels;
using Accelerator.Commercetools.Importer.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Commercetools.Importer.Shared.Consumer;

public class LandingDataConsumer<T, TContext> : ILandingDataService, IService 
        where T : class, new()
        where TContext : DbContext
{
    private readonly CancellationTokenSource? _cancellationTokenSource = new();
    private readonly IFileConsumer<T> _fileConsumer;
    private readonly IDataBaseService<T, TContext> _dataBaseService;
    private readonly ILogger<LandingDataConsumer<T, TContext>> _logger;
    private readonly TContext _context;
    private readonly DataBaseProcessingChannel<T> _dataBaseProcessingChannel;

    public LandingDataConsumer(IFileConsumer<T> fileConsumer,
        IDataBaseService<T, TContext> dataBaseService,
        ILogger<LandingDataConsumer<T, TContext>> logger,
        TContext context,
        DataBaseProcessingChannel<T> dataBaseProcessingChannel)
    { 
        _fileConsumer = fileConsumer;
        _dataBaseService = dataBaseService;
        _logger = logger;
        _context = context;
        _dataBaseProcessingChannel = dataBaseProcessingChannel;
    }

    public async Task DoWorkAsync()
    {
        var timer = new Stopwatch();
        timer.Start();

        var dataConsumers = _fileConsumer.StartFileConsumers().ToList();
        var savingTasks = _dataBaseService.StartConsumersForDatabase().ToList();

        if (_cancellationTokenSource != null)
        {
            var fileReadingTask = Task.Run(_fileConsumer.ReadAsync, _cancellationTokenSource.Token);

            var dataConsumersCompletionTask = Task.WhenAll(dataConsumers)
                .ContinueWith(_ => _dataBaseProcessingChannel.ProcessingChannel.Writer.Complete());

            await Task.WhenAll(fileReadingTask, dataConsumersCompletionTask, Task.WhenAll(savingTasks));
        }

        timer.Stop();
        _logger.LogInformation("finsihed import Time taken: {S}", timer.Elapsed.ToString(@"m\:ss\.fff"));
    }

    public async Task TruncateTable()
    {
        _logger.LogInformation(@"Truncate table: {0}", typeof(T).Name);
        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
        await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{typeof(T).Name}\" RESTART IDENTITY CASCADE;");
        scope.Complete();
    }
}