using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Accelerator.Shared.Infrastructure.Infrastructure;

public partial class LandingImportContext : DbContext
{
    // public DbSet<Prices> Prices { get; set; }
    // public DbSet<Inventory> Inventories { get; set; }
    // public DbSet<Category> Categories { get; set; }
    
    private static ILoggerFactory ContextLoggerFactory => LoggerFactory.Create(
        i => i
            .AddConsole()
            // .AddFilter("", LogLevel.Error)
            // .AddFilter("", LogLevel.Critical)
            .AddFilter("", LogLevel.Warning)
    );

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // todo: inject ConnectionString instead of plain text
        optionsBuilder.UseNpgsql(@"");
            // .LogTo(message => Debug.WriteLine(message));

        // optionsBuilder.UseMySQL(@"Server=192.168.2.27;Port=3306;Database=Accelerator;Uid=root;Password=root")
        //     .EnableSensitiveDataLogging()
        //     .UseLoggerFactory(ContextLoggerFactory);
    }
}