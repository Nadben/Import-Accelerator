using Accelerator.Shared.Infrastructure.Infrastructure;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Accelerator.Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Configuration configuration)
    {
        services.AddDbContext<LandingImportContext>();
        services.AddDbContext<StagingImportContext>();
        services.AddDbContextFactory<LandingImportContext>();
        services.AddDbContextFactory<StagingImportContext>();
        services.AddMapster();

        return services;
    }
}