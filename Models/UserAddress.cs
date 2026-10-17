using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("user_addresses", Schema = "CSI402DB")]
[Index("UserId", Name = "fk_user_addresses_user")]
public partial class UserAddress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("recipient_name")]
    [StringLength(100)]
    public string RecipientName { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Column("address_line1")]
    [StringLength(255)]
    public string AddressLine1 { get; set; } = null!;

    [Column("address_line2")]
    [StringLength(255)]
    public string? AddressLine2 { get; set; }

    [Column("subdistrict")]
    [StringLength(100)]
    public string Subdistrict { get; set; } = null!;

    [Column("district")]
    [StringLength(100)]
    public string District { get; set; } = null!;

    [Column("province")]
    [StringLength(100)]
    public string Province { get; set; } = null!;

    [Column("postal_code")]
    [StringLength(10)]
    public string PostalCode { get; set; } = null!;

    [Column("is_default")]
    public bool IsDefault { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserAddresses")]
    public virtual User User { get; set; } = null!;
}
