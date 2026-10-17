using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("payments", Schema = "CSI402DB")]
[Index("OrderId", Name = "order_id", IsUnique = true)]
public partial class Payment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("transaction_id")]
    [StringLength(100)]
    public string? TransactionId { get; set; }

    [Column("slip_image_url")]
    [StringLength(500)]
    public string? SlipImageUrl { get; set; }

    [Column("paid_at", TypeName = "timestamp")]
    public DateTime? PaidAt { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payment")]
    public virtual Order Order { get; set; } = null!;
}
