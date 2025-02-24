using Accelerator.Schema.Generator.Parsers;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Accelerator.Schema.Generator;

[Generator(LanguageNames.CSharp)]
public class EntityModelGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var pipeline = context.AdditionalTextsProvider
            .Where(static text => Path.GetExtension(text.Path) is ".txt" or ".csv" or ".json" or ".xml")
            .Select(static (text, cancellationToken) =>
                {
                    var extension = Path.GetExtension(text.Path);
                    var content = text.GetText(cancellationToken)?.ToString() ?? string.Empty;
                    var fileName = Path.GetFileNameWithoutExtension(text.Path);
                    var lastDirectoryName = Path.GetFileName(Path.GetDirectoryName(text.Path)) ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return null;
                    }
                    
                    try
                    {
                        var manager = new SchemaParserManager();
                        return manager.ParseFile(content, extension, fileName, lastDirectoryName);
                    }
                    catch
                    {
                        return null;
                    }
                })
            .Where(i => i != null)
            .Collect();

        context.RegisterSourceOutput(pipeline,
            static (context, schemas) =>
            {
                foreach (var schema in schemas)
                {
                    if (schema is null)
                    {
                        continue;
                    }

                    var code = PocoClass.Generate(schema.ClassName, schema.Properties);
                    context.AddSource($"{schema.ClassName}Generated.g.cs", SourceText.From(code, Encoding.UTF8));
                }

                if (!schemas.Any()) return;
                var dbContextCode = DbContext.Generate(schemas);
                context.AddSource("LandingImportContext.g.cs", SourceText.From(dbContextCode, Encoding.UTF8));
            }
        );
    }
}