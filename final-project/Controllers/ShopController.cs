using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class ShopController : Controller
{
    private readonly FinalCSI402DB _db;

    public ShopController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(ShopList));
    }

    [HttpGet]
    public IActionResult ShopList()
    {
        var shops = _db.Shops.Include(s => s.User).ToList();
        return View(shops);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var shop = _db.Shops
            .Include(s => s.User)
            .Include(s => s.SocialLinks)
            .FirstOrDefault(s => s.ShopId == id);
        return View(shop);
    }

    [HttpPost]
    public IActionResult AddSocialLink(ShopSocialLink model)
    {
        _db.ShopSocialLinks.Add(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = model.ShopId });
    }

    [HttpPost]
    public IActionResult DeleteSocialLink(int id, int shopId)
    {
        var link = _db.ShopSocialLinks.Find(id);
        if (link != null)
        {
            _db.ShopSocialLinks.Remove(link);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(Details), new { id = shopId });
    }

    // สมัครเป็นร้านค้า (เหมือน Lazada Seller Registration)
    [HttpGet]
    public IActionResult RegisterShop()
    {
        ViewBag.Users = _db.Users.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult RegisterShop(Shop model)
    {
        model.CreatedAt = DateTime.Now;
        _db.Shops.Add(model);
        _db.SaveChanges();

        var wallet = new ShopWallet
        {
            ShopId = model.ShopId,
            Balance = 0,
            UpdatedAt = DateTime.Now
        };
        _db.ShopWallets.Add(wallet);
        _db.Notifications.Add(new Notification
        {
            Title = "ร้านค้าใหม่",
            Message = $"ร้าน {model.ShopName} สมัครเปิดร้านค้าเรียบร้อยแล้ว",
            IsRead = false,
            CreatedAt = DateTime.Now
        });
        _db.SaveChanges();

        return RedirectToAction(nameof(ShopList));
    }

    [HttpGet]
    public IActionResult EditShop(int id)
    {
        ViewBag.Users = _db.Users.ToList();
        var shop = _db.Shops.Find(id);
        return View(shop);
    }

    [HttpPost]
    public IActionResult EditShop(Shop model)
    {
        _db.Shops.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(ShopList));
    }

    [HttpPost]
    public IActionResult DeleteShop(int id)
    {
        var shop = _db.Shops.Find(id);
        if (shop != null)
        {
            _db.Shops.Remove(shop);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(ShopList));
    }
}
