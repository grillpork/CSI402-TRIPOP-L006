using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tripop_8021_lab006.Models;

[Table("user")]
public class User
{
    [Key]
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }
}
