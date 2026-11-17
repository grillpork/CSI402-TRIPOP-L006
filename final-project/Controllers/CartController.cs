using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class CartController : Controller
{
    private readonly FinalCSI402DB _db;

    public CartController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(CartList));
    }

    [HttpGet]
    public IActionResult CartList()
    {
        var carts = _db.Carts.Include(c => c.Product).ToList();
        return View(carts);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var cart = _db.Carts.Include(c => c.Product).FirstOrDefault(c => c.CartId == id);
        return View(cart);
    }

    [HttpGet]
    public IActionResult CreateCart()
    {
        ViewBag.Products = _db.Products.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult CreateCart(Cart model, bool buyNow = false)
    {
        _db.Carts.Add(model);
        _db.SaveChanges();

        if (buyNow)
        {
            return RedirectToAction("Checkout", "Order");
        }

        return RedirectToAction(nameof(CartList));
    }

    [HttpGet]
    public IActionResult EditCart(int id)
    {
        ViewBag.Products = _db.Products.ToList();
        var cart = _db.Carts.Find(id);
        return View(cart);
    }

    [HttpPost]
    public IActionResult EditCart(Cart model)
    {
        _db.Carts.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(CartList));
    }

    [HttpPost]
    public IActionResult DeleteCart(int id)
    {
        var cart = _db.Carts.Find(id);
        if (cart != null)
        {
            _db.Carts.Remove(cart);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(CartList));
    }
}
