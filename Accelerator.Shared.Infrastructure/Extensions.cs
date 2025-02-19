using Accelerator.Shared.Infrastructure.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accelerator.Shared.Infrastructure;

public static class Extensions
{
    private static ILoggerFactory ContextLoggerFactory => LoggerFactory.Create(
        i => i.AddDebug()
            .AddFilter("", LogLevel.Error)
            .AddFilter("", LogLevel.Critical)
            .AddFilter("", LogLevel.Warning)
    );
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        AcceleratorConfiguration acceleratorConfiguration)
    {
        
        services.AddDbContext<LandingImportContext>(opts =>
            {
                opts.UseNpgsql(acceleratorConfiguration.PostgresConnectionString)
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(ContextLoggerFactory);
            }
          );
        
        services.AddDbContext<StagingImportContext>(opts =>
        {
            opts.UseNpgsql(acceleratorConfiguration.PostgresConnectionString)
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(ContextLoggerFactory);
        });
        
        services.AddDbContextFactory<LandingImportContext>();
        services.AddDbContextFactory<StagingImportContext>();
        services.AddMapster();

        return services;
    }
}