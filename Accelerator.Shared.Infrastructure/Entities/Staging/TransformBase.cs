using System.ComponentModel.DataAnnotations;

namespace Accelerator.Shared.Infrastructure.Entities.Staging;

public class TransformBase
{
    [MaxLength(150)] 
    public string Hash { get; set; } = string.Empty;
    
    [MaxLength(150)] 
    public string PreviousHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}