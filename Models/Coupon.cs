using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("coupons", Schema = "CSI402DB")]
[Index("Code", Name = "code", IsUnique = true)]
[Index("ShopId", Name = "fk_coupons_shop")]
public partial class Coupon
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shop_id")]
    public int? ShopId { get; set; }

    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Column("discount_type")]
    [StringLength(20)]
    public string DiscountType { get; set; } = null!;

    [Column("discount_value")]
    public decimal DiscountValue { get; set; }

    [Column("min_order_amount")]
    public decimal MinOrderAmount { get; set; }

    [Column("max_discount_amount")]
    public decimal? MaxDiscountAmount { get; set; }

    [Column("usage_limit")]
    public int UsageLimit { get; set; }

    [Column("used_count")]
    public int UsedCount { get; set; }

    [Column("start_date", TypeName = "timestamp")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "timestamp")]
    public DateTime EndDate { get; set; }

    [Required]
    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("Coupons")]
    public virtual Shop? Shop { get; set; }
}
