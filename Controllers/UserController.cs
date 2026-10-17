using Microsoft.AspNetCore.Mvc;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class UserController : Controller
{
    private static readonly List<UserModel> _users = new()
    {
        new UserModel
        {
            Id = 1,
            Name = "Tripop Saibut",
            Email = "tripopsaibut@gmail.com",
            Role = "Admin",
            Phone = "081-234-5678",
            CreatedAt = DateTime.Now.AddDays(-2),
        },
        new UserModel
        {
            Id = 2,
            Name = "Somsri ",
            Email = "somsri@gmail.com",
            Role = "User",
            Phone = "089-876-5432",
            CreatedAt = DateTime.Now.AddDays(-1),
        },
    };

    [HttpGet]
    public IActionResult Index() => RedirectToAction(nameof(Userlist));

    [HttpGet]
    public IActionResult Userlist()
    {
        return View(_users);
    }

    [HttpGet]
    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddUser(string name, string email, string phone, string role)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            ViewBag.ErrorMessage = "กรุณากรอกชื่อและอีเมลให้ครบถ้วน";
            return View();
        }

        var newUser = new UserModel
        {
            Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1,
            Name = name,
            Email = email,
            Phone = phone,
            Role = string.IsNullOrEmpty(role) ? "User" : role,
            CreatedAt = DateTime.Now,
        };

        _users.Add(newUser);
        TempData["SuccessMessage"] = "เพิ่มผู้ใช้ " + newUser.Name + " เรียบร้อยแล้ว!";
        return RedirectToAction(nameof(Userlist));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            TempData["SuccessMessage"] = "ลบผู้ใช้เรียบร้อยแล้ว";
        }
        return RedirectToAction(nameof(Userlist));
    }
}
