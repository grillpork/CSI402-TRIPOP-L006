using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Helpers;
using final_project.Models;

namespace final_project.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly FinalCSI402DB _db;
    private readonly IWebHostEnvironment _env;

    public UserController(FinalCSI402DB db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(UserList));
    }

    [HttpGet]
    public IActionResult UserList(string? q = null, int? roleId = null)
    {
        var allUsers = _db.Users.Include(u => u.Role).ToList();
        ViewBag.TotalUsers = allUsers.Count;
        ViewBag.AdminCount = allUsers.Count(u => u.Role?.RoleName == "Admin");
        ViewBag.SellerCount = allUsers.Count(u => u.Role?.RoleName == "Seller");
        ViewBag.ClientCount = allUsers.Count(u => u.Role == null || u.Role.RoleName == "Client");
        ViewBag.Roles = _db.Roles.ToList();
        ViewBag.SearchQuery = q;
        ViewBag.SelectedRoleId = roleId;

        var query = allUsers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var keyword = q.Trim().ToLower();
            query = query.Where(u =>
                (u.UserName != null && u.UserName.ToLower().Contains(keyword)) ||
                (u.Email != null && u.Email.ToLower().Contains(keyword)));
        }

        if (roleId.HasValue)
        {
            query = query.Where(u => u.RoleId == roleId.Value);
        }

        return View(query.OrderByDescending(u => u.UserId).ToList());
    }

    [HttpPost]
    public IActionResult ChangeRole(int userId, int roleId)
    {
        var user = _db.Users.Find(userId);
        if (user != null)
        {
            user.RoleId = roleId;
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(UserList));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var user = _db.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == id);
        return View(user);
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        ViewBag.Roles = _db.Roles.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult CreateUser(UserViewModel model, IFormFile? imageFile)
    {
        var user = new User
        {
            UserName = model.UserName,
            Password = model.Password,
            Email = model.Email,
            RoleId = model.RoleId ?? 3
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        var media = MediaHelper.SaveMedia(imageFile, _env, _db, "User", user.UserId);
        if (media != null)
        {
            user.ImageUrl = media.FilePath;
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(UserList));
    }

    [HttpGet]
    public IActionResult EditUser(int id)
    {
        ViewBag.Roles = _db.Roles.ToList();
        var user = _db.Users.Find(id);
        return View(user);
    }

    [HttpPost]
    public IActionResult EditUser(User model, IFormFile? imageFile)
    {
        var media = MediaHelper.SaveMedia(imageFile, _env, _db, "User", model.UserId);
        if (media != null)
        {
            model.ImageUrl = media.FilePath;
        }

        _db.Users.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(UserList));
    }

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
