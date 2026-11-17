using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("user")]
public class User
{
    [Key]
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? ImageUrl { get; set; }

    public int? RoleId { get; set; } = 3;

    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }
}
