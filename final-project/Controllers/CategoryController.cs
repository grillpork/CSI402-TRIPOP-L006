using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;

namespace final_project.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly FinalCSI402DB _db;

    public CategoryController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(CategoryList));
    }

    [HttpGet]
    public IActionResult CategoryList()
    {
        var categories = _db.Categories.ToList();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var category = _db.Categories.Find(id);
        return View(category);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(Category model)
    {
        _db.Categories.Add(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(CategoryList));
    }

    [HttpGet]
    public IActionResult EditCategory(int id)
    {
        var category = _db.Categories.Find(id);
        return View(category);
    }

    [HttpPost]
    public IActionResult EditCategory(Category model)
    {
        _db.Categories.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(CategoryList));
    }

    [HttpPost]
    public IActionResult DeleteCategory(int id)
    {
        var category = _db.Categories.Find(id);
        if (category != null)
        {
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(CategoryList));
    }
}
