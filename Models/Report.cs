using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("reports", Schema = "CSI402DB")]
[Index("ReporterUserId", Name = "fk_reports_reporter")]
public partial class Report
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reporter_user_id")]
    public int ReporterUserId { get; set; }

    [Column("target_type")]
    [StringLength(50)]
    public string TargetType { get; set; } = null!;

    [Column("target_id")]
    public int TargetId { get; set; }

    [Column("reason")]
    [StringLength(100)]
    public string Reason { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Column("evidence_image_url")]
    [StringLength(500)]
    public string? EvidenceImageUrl { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("ReporterUserId")]
    [InverseProperty("Reports")]
    public virtual User ReporterUser { get; set; } = null!;
}
