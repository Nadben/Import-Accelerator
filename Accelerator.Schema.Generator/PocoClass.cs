using System.Text;
using Accelerator.Schema.Generator.Helper;

namespace Accelerator.Schema.Generator;

public static class PocoClass
{
    public static string Generate(string className, Dictionary<string, Type> properties)
    {
        var propertiesCode = new StringBuilder();

        foreach (var property in properties)
        {
            propertiesCode.AppendLine($"\t\tpublic {TypeAlias.Aliases[property.Value]} {property.Key} {{ get; set; }}");
        }

        return $$"""
                 using System;

                 namespace Accelerator.Shared.Infrastructure.Entities.Landing.Generated
                 {
                     public class {{className}}Generated
                     {
                         public int Id { get; set; }
                 {{propertiesCode.ToString().TrimEnd()}}
                     }
                 }
                 """;
    }
}