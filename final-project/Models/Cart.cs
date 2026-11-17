using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("cart")]
public class Cart
{
    [Key]
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}
