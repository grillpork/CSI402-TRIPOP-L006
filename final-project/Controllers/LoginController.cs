using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class LoginController : Controller
{
    private readonly FinalCSI402DB _db;

    public LoginController(FinalCSI402DB db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel model)
    {
        var user = _db.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => (u.UserName == model.UserName || u.Email == model.UserName) && u.Password == model.Password);

        if (user == null)
        {
            ViewBag.ErrorMessage = "UserName หรือ Password ไม่ถูกต้อง";
            return View(model);
        }

        var roleName = user.Role?.RoleName ?? "Client";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? "User"),
            new Claim(ClaimTypes.Role, roleName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        if (roleName == "Admin")
        {
            return RedirectToAction("Dashboard", "Admin");
        }
        if (roleName == "Seller")
        {
            return RedirectToAction("ProductList", "Product");
        }

        return RedirectToAction("ProductList", "Product");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(UserViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            ViewBag.ErrorMessage = "กรุณากรอก Email และ Password ให้ครบถ้วน";
            return View(model);
        }

        var email = model.Email.Trim();
        var user = new User
        {
            UserName = email,
            Password = model.Password,
            Email = email,
            RoleId = 3
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        TempData["SuccessMessage"] = "สมัครสมาชิกสำเร็จ! กรุณาเข้าสู่ระบบด้วยบัญชีของคุณ";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
