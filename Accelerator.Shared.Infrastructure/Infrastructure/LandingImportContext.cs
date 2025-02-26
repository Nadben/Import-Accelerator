using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Accelerator.Shared.Infrastructure.Infrastructure;

public partial class LandingImportContext(DbContextOptions<LandingImportContext> options) : DbContext(options);