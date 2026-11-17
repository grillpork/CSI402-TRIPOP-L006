using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("notification")]
public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
