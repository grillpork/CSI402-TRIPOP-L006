using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class AuthController : Controller
{
    private readonly Csi402dbContext _db;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthController(Csi402dbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, bool rememberMe = false, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        ViewBag.Email = email;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.ErrorMessage = "กรุณากรอกอีเมลหรือชื่อผู้ใช้ และรหัสผ่าน";
            return View();
        }

        var normalizedInput = email.Trim().ToLower();

        // Search user by email or username
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedInput || u.Username.ToLower() == normalizedInput);

        if (user == null)
        {
            ViewBag.ErrorMessage = "ไม่พบบัญชีผู้ใช้นี้ หรือข้อมูลเข้าสู่ระบบไม่ถูกต้อง";
            return View();
        }

        bool isValid = false;
        try
        {
            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            isValid = (verifyResult == PasswordVerificationResult.Success || verifyResult == PasswordVerificationResult.SuccessRehashNeeded);
        }
        catch
        {
            isValid = false;
        }

        if (!isValid && user.Password == password)
        {
            isValid = true;
        }

        if (!isValid)
        {
            ViewBag.ErrorMessage = "รหัสผ่านไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง";
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        if (!string.IsNullOrEmpty(user.FirstName))
        {
            claims.Add(new(ClaimTypes.GivenName, user.FirstName));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(3)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        TempData["SuccessMessage"] = $"ยินดีต้อนรับกลับ, {user.Username}!";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string email, string password, string confirmPassword)
    {
        ViewBag.Email = email;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.ErrorMessage = "กรุณากรอกอีเมลและรหัสผ่านให้ครบถ้วน";
            return View();
        }

        if (password != confirmPassword)
        {
            ViewBag.ErrorMessage = "รหัสผ่านและการยืนยันรหัสผ่านไม่ตรงกัน";
            return View();
        }

        if (password.Length < 6)
        {
            ViewBag.ErrorMessage = "รหัสผ่านต้องมีความยาวอย่างน้อย 6 ตัวอักษร";
            return View();
        }

        var normalizedEmail = email.Trim().ToLower();

        if (await _db.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail))
        {
            ViewBag.ErrorMessage = "อีเมลนี้ถูกลงทะเบียนไว้แล้ว กรุณาเข้าสู่ระบบหรือใช้อีเมลอื่น";
            return View();
        }

        var baseUsername = normalizedEmail.Split('@')[0];
        var username = baseUsername;
        int counter = 1;
        while (await _db.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            username = $"{baseUsername}{counter++}";
        }

        var userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (userRole == null)
        {
            userRole = new Role
            {
                Name = "User",
                CreatedAt = DateTime.UtcNow
            };
            _db.Roles.Add(userRole);
            await _db.SaveChangesAsync();
        }

        var newUser = new User
        {
            Username = username,
            Email = normalizedEmail,
            RoleId = userRole.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        newUser.Password = _passwordHasher.HashPassword(newUser, password);

        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "สมัครสมาชิกสำเร็จ! สามารถเข้าสู่ระบบได้ทันที";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "ออกจากระบบเรียบร้อยแล้ว";
        return RedirectToAction("Index", "Home");
    }
}
