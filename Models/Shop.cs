using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("shops", Schema = "CSI402DB")]
[Index("UserId", Name = "user_id", IsUnique = true)]
public partial class Shop
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Column("logo_url")]
    [StringLength(500)]
    public string? LogoUrl { get; set; }

    [Column("banner_url")]
    [StringLength(500)]
    public string? BannerUrl { get; set; }

    [Column("rating")]
    [Precision(3)]
    public decimal Rating { get; set; }

    [Column("is_verified")]
    public bool IsVerified { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("Shop")]
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    [InverseProperty("Shop")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Shop")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Shop")]
    public virtual ICollection<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();

    [InverseProperty("Shop")]
    public virtual ShopWallet? ShopWallet { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Shop")]
    public virtual User User { get; set; } = null!;
}
