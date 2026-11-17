using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("shop_wallet")]
public class ShopWallet
{
    [Key]
    public int WalletId { get; set; }

    public int ShopId { get; set; }

    public decimal Balance { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey("ShopId")]
    public virtual Shop? Shop { get; set; }

    public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}
