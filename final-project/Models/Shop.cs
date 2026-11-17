using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("shop")]
public class Shop
{
    [Key]
    public int ShopId { get; set; }

    public int UserId { get; set; }

    public string? ShopName { get; set; }

    public string? Description { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public virtual ICollection<ShopSocialLink> SocialLinks { get; set; } = new List<ShopSocialLink>();
}
