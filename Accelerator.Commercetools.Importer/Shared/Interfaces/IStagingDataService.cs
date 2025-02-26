using Microsoft.EntityFrameworkCore;

namespace Accelerator.Commercetools.Importer.Shared.Interfaces;

public interface IStagingDataService<TLandingEntity, TLandingContext, TStagingEntity, TStagingContext>
    where TLandingEntity : class, new()
    where TStagingEntity : class, new()
    where TLandingContext : DbContext
    where TStagingContext : DbContext
{
    public ParallelQuery<TStagingEntity> Transform();
    public Task ResetHashCodeForTable();
}