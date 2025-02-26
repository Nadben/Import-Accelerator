using Accelerator.Shared.Infrastructure.Entities.Staging;
using Microsoft.EntityFrameworkCore;

namespace Accelerator.Shared.Infrastructure.Infrastructure;

public class StagingImportContext(DbContextOptions<StagingImportContext> options) : DbContext(options)
{
    public DbSet<CommercetoolsStandalonePriceImport> CommercetoolsStandalonePrice { get; set; }
    public DbSet<CommercetoolsCategoryImport> CommercetoolsCategoryImports { get; set; }
    public DbSet<CommercetoolsInventoryImport> CommercetoolsInventoryImports { get; set; }
}