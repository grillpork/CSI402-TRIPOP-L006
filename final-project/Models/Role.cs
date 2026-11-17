using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("role")]
public class Role
{
    [Key]
    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
