using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tripop_8021_lab006.Models;

[Table("LabStudent", Schema = "CSI402DB")]
public partial class LabStudent
{
    [Key]
    [StringLength(10)]
    public string StdId { get; set; } = null!;

    [StringLength(30)]
    public string? StdPassword { get; set; }

    [StringLength(50)]
    public string? StdName { get; set; }

    [StringLength(100)]
    public string? StdLastname { get; set; }
}
