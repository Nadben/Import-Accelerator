using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using Accelerator.Commercetools.Importer.Commercetools;
using Accelerator.Commercetools.Importer.GenericEtlPipeline;
using Accelerator.Commercetools.Importer.Mapping;
using Accelerator.Commercetools.Importer.Shared.Channels;
using Accelerator.Commercetools.Importer.Shared.Consumer;
using Accelerator.Commercetools.Importer.Shared.Interfaces;
using Accelerator.Commercetools.Importer.Shared.Services;
using Accelerator.Commercetools.Importer.Workflow;
using Accelerator.Shared.Infrastructure;
using commercetools.Sdk.Api;
using commercetools.Sdk.ImportApi;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;

var builder = Host.CreateApplicationBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

IConfiguration configuration = configurationBuilder.Build();
var opts = new AcceleratorConfiguration();
var config = configuration.GetSection("AcceleratorConfiguration");
config.Bind(opts, o => o.BindNonPublicProperties = true);


builder.Services.AddLogging();
builder.Services.Configure<AcceleratorConfiguration>(config);
builder.Services.AddInfrastructure(opts);
builder.Services.AddCommercetools(configuration);
builder.Services.AddScoped(typeof(FileProcessingChannel<>));
builder.Services.AddScoped(typeof(DataBaseProcessingChannel<>));
builder.Services.AddScoped(typeof(BlockingCollection<>));
builder.Services.AddTransient(typeof(IFileConsumer<>), typeof(FileConsumer<>));
builder.Services.AddTransient(typeof(IDataBaseService<,>), typeof(DataBaseService<,>));
builder.Services.AddTransient(typeof(LandingDataConsumer<,>));
builder.Services.AddTransient(typeof(StagingDataConsumer<,,,>));
builder.Services.AddTransient<IImportApi, ImportApi>();

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