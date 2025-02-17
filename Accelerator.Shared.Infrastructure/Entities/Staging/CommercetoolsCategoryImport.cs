using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Accelerator.Shared.Infrastructure.Entities.Staging;

public class CommercetoolsCategoryImport : TransformBase
{
    [IgnoreDataMember]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Slug { get; set; } = string.Empty;
}