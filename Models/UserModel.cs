using System.ComponentModel.DataAnnotations;

namespace tripop_8021_lab006.Models;

public class UserModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "กรุณากรอกชื่อ-นามสกุล")]
    [Display(Name = "ชื่อ-นามสกุล")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณากรอกอีเมล")]
    [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
    [Display(Name = "อีเมล")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "กรุณาเลือกบทบาท")]
    [Display(Name = "บทบาท (Role)")]
    public string Role { get; set; } = "User";

    [Display(Name = "เบอร์โทรศัพท์")]
    public string? Phone { get; set; }

    [Display(Name = "วันที่สร้าง")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
