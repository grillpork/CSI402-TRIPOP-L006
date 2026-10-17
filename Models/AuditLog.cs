using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("audit_logs", Schema = "CSI402DB")]
[Index("UserId", Name = "fk_audit_logs_user")]
[Index("Action", Name = "idx_audit_action")]
[Index("CreatedAt", Name = "idx_audit_created_at")]
[Index("EntityName", "EntityId", Name = "idx_audit_entity")]
public partial class AuditLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("action")]
    [StringLength(100)]
    public string Action { get; set; } = null!;

    [Column("http_method")]
    [StringLength(10)]
    public string? HttpMethod { get; set; }

    [Column("api_route")]
    [StringLength(255)]
    public string? ApiRoute { get; set; }

    [Column("entity_name")]
    [StringLength(100)]
    public string EntityName { get; set; } = null!;

    [Column("entity_id")]
    public int? EntityId { get; set; }

    [Column("old_values", TypeName = "text")]
    public string? OldValues { get; set; }

    [Column("new_values", TypeName = "text")]
    public string? NewValues { get; set; }

    [Column("ip_address")]
    [StringLength(45)]
    public string? IpAddress { get; set; }

    [Column("user_agent")]
    [StringLength(500)]
    public string? UserAgent { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual User? User { get; set; }
}
