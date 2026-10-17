using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("order_items", Schema = "CSI402DB")]
[Index("OrderId", Name = "fk_order_items_order")]
[Index("ProductId", Name = "fk_order_items_product")]
[Index("ShopOrderId", Name = "fk_order_items_shop_order")]
[Index("ProductVariantId", Name = "fk_order_items_variant")]
public partial class OrderItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("shop_order_id")]
    public int? ShopOrderId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("product_variant_id")]
    public int? ProductVariantId { get; set; }

    [Column("product_name")]
    [StringLength(200)]
    public string ProductName { get; set; } = null!;

    [Column("variant_info")]
    [StringLength(200)]
    public string? VariantInfo { get; set; }

    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderItems")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("OrderItems")]
    public virtual ProductVariant? ProductVariant { get; set; }

    [ForeignKey("ShopOrderId")]
    [InverseProperty("OrderItems")]
    public virtual ShopOrder? ShopOrder { get; set; }
}
