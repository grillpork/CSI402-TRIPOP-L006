using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("order_item")]
public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Price { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}
