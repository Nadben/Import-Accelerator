namespace Accelerator.Shared.Infrastructure.Entities.Landing;

public class Prices
{
    public int Id { get; set; }
    public string Mfg { get; set; } = string.Empty;
    public string Subline { get; set; } = string.Empty;
    public string Part { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Core { get; set; }
    public decimal EHFee { get; set; } 
    public string RegionID { get; set; } = string.Empty;
} 