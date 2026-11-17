using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("payment")]
public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; } = "Paid";

    public DateTime PaidAt { get; set; } = DateTime.Now;

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
}
