using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("shop_wallets", Schema = "CSI402DB")]
[Index("ShopId", Name = "shop_id", IsUnique = true)]
public partial class ShopWallet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shop_id")]
    public int ShopId { get; set; }

    [Column("balance")]
    public decimal Balance { get; set; }

    [Column("pending_balance")]
    public decimal PendingBalance { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("ShopWallet")]
    public virtual Shop Shop { get; set; } = null!;

    [InverseProperty("Wallet")]
    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
}
