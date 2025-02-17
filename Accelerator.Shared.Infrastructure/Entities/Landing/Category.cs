namespace Accelerator.Shared.Infrastructure.Entities.Landing;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public List<Category> SubCategory { get; set; } = new();
}