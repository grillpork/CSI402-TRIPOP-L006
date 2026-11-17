using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class OrderController : Controller
{
    private readonly FinalCSI402DB _db;

    public OrderController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(OrderList));
    }

    [HttpGet]
    public IActionResult OrderList()
    {
        var orders = _db.Orders
            .Include(o => o.User)
            .OrderByDescending(o => o.OrderId)
            .ToList();
        return View(orders);
    }

    [HttpGet]
    public IActionResult OrderDetails(int id)
    {
        var order = _db.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefault(o => o.OrderId == id);
        return View(order);
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var carts = _db.Carts.Include(c => c.Product).ToList();
        ViewBag.Users = _db.Users.ToList();
        ViewBag.Carts = carts;
        ViewBag.TotalAmount = carts.Sum(c => (c.Product?.Price ?? 0) * c.Quantity);
        return View();
    }

    [HttpPost]
    public IActionResult Checkout(Order model)
    {
        var carts = _db.Carts.Include(c => c.Product).ToList();
        if (!carts.Any())
        {
            return RedirectToAction("CartList", "Cart");
        }

        model.TotalAmount = carts.Sum(c => (c.Product?.Price ?? 0) * c.Quantity);
        model.Status = "Pending";
        model.CreatedAt = DateTime.Now;

        _db.Orders.Add(model);
        _db.SaveChanges();

        foreach (var item in carts)
        {
            var orderItem = new OrderItem
            {
                OrderId = model.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product?.Price ?? 0
            };
            _db.OrderItems.Add(orderItem);

            if (item.Product != null)
            {
                item.Product.Stock -= item.Quantity;
            }
        }

        _db.Carts.RemoveRange(carts);
        _db.Notifications.Add(new Notification
        {
            Title = $"คำสั่งซื้อใหม่ #{model.OrderId}",
            Message = $"มีคำสั่งซื้อใหม่ยอด {model.TotalAmount:N2} บาท (รอชำระเงิน)",
            IsRead = false,
            CreatedAt = DateTime.Now
        });
        _db.SaveChanges();

        return RedirectToAction("Pay", "Payment", new { orderId = model.OrderId });
    }

    [HttpGet]
    public IActionResult EditOrder(int id)
    {
        ViewBag.Users = _db.Users.ToList();
        var order = _db.Orders.Find(id);
        return View(order);
    }

    [HttpPost]
    public IActionResult EditOrder(Order model)
    {
        _db.Orders.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(OrderList));
    }

    [HttpPost]
    public IActionResult DeleteOrder(int id)
    {
        var order = _db.Orders.Find(id);
        if (order != null)
        {
            _db.Orders.Remove(order);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(OrderList));
    }
}
