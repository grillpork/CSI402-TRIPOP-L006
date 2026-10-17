using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("shop_orders", Schema = "CSI402DB")]
[Index("OrderId", Name = "fk_shop_orders_order")]
[Index("ShopId", Name = "fk_shop_orders_shop")]
public partial class ShopOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [Column("subtotal")]
    public decimal Subtotal { get; set; }

    [Column("shipping_fee")]
    public decimal ShippingFee { get; set; }

    [Column("discount_amount")]
    public decimal DiscountAmount { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("shipping_provider")]
    [StringLength(100)]
    public string? ShippingProvider { get; set; }

    [Column("tracking_number")]
    [StringLength(100)]
    public string? TrackingNumber { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("ShopOrders")]
    public virtual Order Order { get; set; } = null!;

    [InverseProperty("ShopOrder")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("ShopId")]
    [InverseProperty("ShopOrders")]
    public virtual Shop Shop { get; set; } = null!;
}
