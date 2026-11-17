using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models;

[Table("media")]
public class Media
{
    [Key]
    public int MediaId { get; set; }

    public string? FileName { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public string? MediaType { get; set; }

    public string? ContentType { get; set; }

    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    public int SortOrder { get; set; } = 0;

    public DateTime UploadedAt { get; set; } = DateTime.Now;
}
