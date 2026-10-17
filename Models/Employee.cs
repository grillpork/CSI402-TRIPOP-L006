using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

[Table("employees", Schema = "CSI402DB")]
[Index("UserId", Name = "user_id", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("position")]
    [StringLength(100)]
    public string Position { get; set; } = null!;

    [Column("department")]
    [StringLength(100)]
    public string Department { get; set; } = null!;

    [Column("salary")]
    public decimal Salary { get; set; }

    [Column("hire_date", TypeName = "timestamp")]
    public DateTime HireDate { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Employee")]
    public virtual User User { get; set; } = null!;
}
