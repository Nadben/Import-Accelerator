using System.Collections.Concurrent;
using System.Reflection;
using Accelerator.Commercetools.Importer.GenericEtlPipeline;
using Accelerator.Commercetools.Importer.Mapping;
using Accelerator.Commercetools.Importer.Shared.Channels;
using Accelerator.Commercetools.Importer.Shared.Consumer;
using Accelerator.Commercetools.Importer.Shared.Interfaces;
using Accelerator.Commercetools.Importer.Shared.Services;
using Accelerator.Commercetools.Importer.Workflow;
using Accelerator.Shared.Infrastructure;
using commercetools.Sdk.Api;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

IConfiguration configuration = configurationBuilder.Build();
var opts = new AcceleratorConfiguration();
var config = configuration.GetSection("AcceleratorConfiguration");
config.Bind(opts, o => o.BindNonPublicProperties = true);

// commercetools config
builder.Services.UseCommercetoolsApi(configuration, "AcceleratorConfiguration:CommercetoolsConfig");
builder.Services.AddLogging();

builder.Services.Configure<AcceleratorConfiguration>(config);
builder.Services.AddInfrastructure(opts);
builder.Services.AddScoped(typeof(FileProcessingChannel<>));
builder.Services.AddScoped(typeof(DataBaseProcessingChannel<>));
builder.Services.AddScoped(typeof(BlockingCollection<>));
builder.Services.AddTransient(typeof(IFileConsumer<>), typeof(FileConsumer<>));
builder.Services.AddTransient(typeof(IDataBaseService<,>), typeof(DataBaseService<,>));
builder.Services.AddTransient(typeof(LandingDataConsumer<,>));
builder.Services.AddTransient(typeof(StagingDataConsumer<,,,>));
builder.Services.AddTransient(typeof(LandingWorker<,>));
builder.Services.AddTransient(typeof(StagingWorker<,,,>));

var assembly = Assembly.GetExecutingAssembly();
builder.Logging.AddConsole();

builder.Services.AddSingleton<TaskQueue>();

var workerTypes = assembly.GetTypes()
        .Where(type => type.GetCustomAttribute<WorkflowTaskAttribute>() != null);

foreach (var workerType in workerTypes)
{
        builder.Services.AddTransient(workerType);
}
builder.Services.AddHostedService<WorkflowOrchestrator>();

var mapsterConfig = new TypeAdapterConfig();
mapsterConfig.Apply(new CommercetoolsMapper());
// mapsterConfig.Scan(Assembly.GetExecutingAssembly()); 
builder.Services.AddSingleton(mapsterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();
// TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetEntryAssembly());

using IHost host = builder.Build();
host.Run();

