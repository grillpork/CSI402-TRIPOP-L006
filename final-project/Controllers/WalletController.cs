using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

[Authorize(Roles = "Admin,Seller")]
public class WalletController : Controller
{
    private readonly FinalCSI402DB _db;

    public WalletController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(WalletList));
    }

    [HttpGet]
    public IActionResult WalletList()
    {
        var wallets = _db.ShopWallets
            .Include(w => w.Shop)
                .ThenInclude(s => s!.User)
            .ToList();
        return View(wallets);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var wallet = _db.ShopWallets
            .Include(w => w.Shop)
                .ThenInclude(s => s!.User)
            .Include(w => w.Transactions.OrderByDescending(t => t.TransactionId))
            .FirstOrDefault(w => w.WalletId == id);
        return View(wallet);
    }
}
