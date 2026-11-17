using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("shop_social_link")]
public class ShopSocialLink
{
    [Key]
    public int SocialLinkId { get; set; }

    public int ShopId { get; set; }

    public string? Platform { get; set; }

    public string? Url { get; set; }

    [ForeignKey("ShopId")]
    public virtual Shop? Shop { get; set; }
}
