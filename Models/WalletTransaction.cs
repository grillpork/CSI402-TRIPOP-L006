using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("wallet_transactions", Schema = "CSI402DB")]
[Index("WalletId", Name = "fk_wallet_tx_wallet")]
public partial class WalletTransaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("wallet_id")]
    public int WalletId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("type")]
    [StringLength(50)]
    public string Type { get; set; } = null!;

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("reference_id")]
    [StringLength(100)]
    public string? ReferenceId { get; set; }

    [Column("bank_name")]
    [StringLength(100)]
    public string? BankName { get; set; }

    [Column("bank_account_number")]
    [StringLength(50)]
    public string? BankAccountNumber { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("WalletId")]
    [InverseProperty("WalletTransactions")]
    public virtual ShopWallet Wallet { get; set; } = null!;
}
