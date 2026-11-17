using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("orders")]
public class Order
{
    [Key]
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public string? ShippingAddress { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; } = "Completed";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
