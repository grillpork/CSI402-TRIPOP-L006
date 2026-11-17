using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class HomeController : Controller
{
    private readonly FinalCSI402DB _db;

    public HomeController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var products = _db.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.ProductId)
            .Take(6)
            .ToList();
        ViewBag.Categories = _db.Categories.ToList();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
