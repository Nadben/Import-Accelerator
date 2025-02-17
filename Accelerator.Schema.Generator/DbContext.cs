using System.Collections.Immutable;
using System.Text;
using Accelerator.Schema.Generator.Parsers.Model;

namespace Accelerator.Schema.Generator;

public static class DbContext
{
    public static string Generate(ImmutableArray<ClassSchema?> schemas)
    {
        var dbSetsBuilder = new StringBuilder();

        foreach (var classSchema in schemas.OfType<ClassSchema>())
        {
            dbSetsBuilder.AppendLine($"\t\tpublic DbSet<{classSchema.ClassName}Generated> {classSchema.ClassName}Generated {{ get; set; }}");
        }


        return $$"""
                 using Microsoft.EntityFrameworkCore;
                 using Accelerator.Shared.Infrastructure.Entities.Landing.Generated;

                 namespace Accelerator.Shared.Infrastructure.Infrastructure
                 {
                     public partial class LandingImportContext : DbContext
                     {
                 {{dbSetsBuilder.ToString().TrimEnd()}}
                     }
                 }
                 """;
    }
}