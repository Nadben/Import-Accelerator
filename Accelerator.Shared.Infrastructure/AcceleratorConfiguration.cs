namespace Accelerator.Shared.Infrastructure;

public class AcceleratorConfiguration
{
    public string SourceDirectory { get; init; } = string.Empty;
    public string PostgresConnectionString { get; init; } = string.Empty;
    public CommercetoolsConfig CommercetoolsConfig { get; init; } = new();
}

public class CommercetoolsConfig
{
    public string ApiBaseAddress { get; set; } = string.Empty;
    public string AuthorizationBaseAddress { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string ProjectKey { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
}