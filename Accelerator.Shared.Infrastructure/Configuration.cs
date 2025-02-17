namespace Accelerator.Shared.Infrastructure;

public class Configuration
{
    public string SourceDirectory { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; // todo: remove
    public string Separator { get; set; } = string.Empty; // todo: remove
    public string PostgresConnectionString { get; set; } = string.Empty;
}