using Microsoft.AspNetCore.Mvc;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class LoginController : Controller
{
    private readonly CSI402DB _db;

    public LoginController(CSI402DB db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(UserViewModel model)
    {
        var user = _db.Users.FirstOrDefault(u =>
            u.UserName == model.UserName && u.Password == model.Password);

        if (user == null)
        {
            ViewBag.ErrorMessage = "UserName หรือ Password ไม่ถูกต้อง";
            return View(model);
        }

        return RedirectToAction("UserList", "User");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(UserViewModel model)
    {
        var user = new User
        {
            UserName = model.UserName,
            Password = model.Password,
            Email = model.Email
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        return RedirectToAction(nameof(Login));
    }
}
