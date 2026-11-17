using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("wallet_transaction")]
public class WalletTransaction
{
    [Key]
    public int TransactionId { get; set; }

    public int WalletId { get; set; }

    public int? OrderId { get; set; }

    public decimal Amount { get; set; }

    public string? Type { get; set; } = "Income";

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("WalletId")]
    public virtual ShopWallet? Wallet { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
}
