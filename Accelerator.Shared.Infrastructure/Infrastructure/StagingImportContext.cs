using Accelerator.Shared.Infrastructure.Entities.Staging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accelerator.Shared.Infrastructure.Infrastructure;

public class StagingImportContext : DbContext
{
    public DbSet<CommercetoolsStandalonePriceImport> CommercetoolsStandalonePrice { get; set; }
    public DbSet<CommercetoolsCategoryImport> CommercetoolsCategoryImports { get; set; }
    public DbSet<CommercetoolsInventoryImport> CommercetoolsInventoryImports { get; set; }
    
    private static ILoggerFactory ContextLoggerFactory => LoggerFactory.Create(
        i => i.AddConsole()
            // .AddFilter("", LogLevel.Error)
            // .AddFilter("", LogLevel.Critical)
            .AddFilter("", LogLevel.Warning)
    );
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // todo: inject ConnectionString instead of plain text
        optionsBuilder
            .UseNpgsql(@"")
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(ContextLoggerFactory);
        
        // optionsBuilder
        //     .UseMySQL(@"Server=192.168.2.27;Port=3306;Database=Accelerator;Uid=root;Password=root")
        //     .EnableSensitiveDataLogging()
        //     .UseLoggerFactory(ContextLoggerFactory);

    }
}