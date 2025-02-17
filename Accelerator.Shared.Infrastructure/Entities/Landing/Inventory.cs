namespace Accelerator.Shared.Infrastructure.Entities.Landing;

public class Inventory
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string LocationId { get; set; }
    public string Sku { get; set; }
}