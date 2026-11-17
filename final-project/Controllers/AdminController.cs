using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly FinalCSI402DB _db;

    public AdminController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        ViewBag.UserCount = _db.Users.Count();
        ViewBag.ProductCount = _db.Products.Count();
        ViewBag.CategoryCount = _db.Categories.Count();
        ViewBag.OrderCount = _db.Orders.Count();

        ViewBag.RecentUsers = _db.Users
            .Include(u => u.Role)
            .OrderByDescending(u => u.UserId)
            .Take(5)
            .ToList();

        ViewBag.RecentProducts = _db.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.ProductId)
            .Take(5)
            .ToList();

        ViewBag.Categories = _db.Categories
            .Include(c => c.Products)
            .ToList();

        return View();
    }
}
