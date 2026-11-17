using Microsoft.AspNetCore.Mvc;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class UserController : Controller
{
    private readonly CSI402DB _db;

    public UserController(CSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(UserList));
    }

    // READ: แสดงรายการ User ทั้งหมด
    [HttpGet]
    public IActionResult UserList()
    {
        var users = _db.Users.ToList();
        return View(users);
    }

    // READ: ดูรายละเอียด User รายคน
    [HttpGet]
    public IActionResult Details(int id)
    {
        var user = _db.Users.Find(id);
        return View(user);
    }

    // CREATE: แสดงหน้าเพิ่ม User
    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    // CREATE: บันทึก User ใหม่ลง DB
    [HttpPost]
    public IActionResult CreateUser(UserViewModel model)
    {
        var user = new User
        {
            UserName = model.UserName,
            Password = model.Password,
            Email = model.Email
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        return RedirectToAction(nameof(UserList));
    }

    // UPDATE: แสดงหน้าแก้ไข User
    [HttpGet]
    public IActionResult EditUser(int id)
    {
        var user = _db.Users.Find(id);
        return View(user);
    }

    // UPDATE: บันทึกการแก้ไข User ลง DB
    [HttpPost]
    public IActionResult EditUser(User model)
    {
        _db.Users.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(UserList));
    }

    // DELETE: ลบ User ออกจาก DB
    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(UserList));
    }
}
