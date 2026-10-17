using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("cart_items", Schema = "CSI402DB")]
[Index("ProductId", Name = "fk_cart_items_product")]
[Index("ProductVariantId", Name = "fk_cart_items_variant")]
[Index("CartId", "ProductId", "ProductVariantId", Name = "uk_cart_item_unique", IsUnique = true)]
public partial class CartItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cart_id")]
    public int CartId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("product_variant_id")]
    public int? ProductVariantId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("CartItems")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("CartItems")]
    public virtual ProductVariant? ProductVariant { get; set; }
}
