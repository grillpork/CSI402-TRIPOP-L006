using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("product")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public int? ShopId { get; set; }

    public string? ProductName { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? Gender { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    [ForeignKey("ShopId")]
    public virtual Shop? Shop { get; set; }
}
