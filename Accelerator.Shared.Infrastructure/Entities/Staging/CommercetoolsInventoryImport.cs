using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Accelerator.Shared.Infrastructure.Entities.Staging;

public class CommercetoolsInventoryImport : TransformBase
{
    [IgnoreDataMember]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(150)]
    public string Sku { get; set; } = string.Empty;

    public int QuantityOnStock { get; set; }

    [MaxLength(150)]
    public string SupplyChannel { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Custom { get; set; } = string.Empty;
}
