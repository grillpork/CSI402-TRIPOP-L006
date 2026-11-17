using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class PaymentController : Controller
{
    private readonly FinalCSI402DB _db;

    public PaymentController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(PaymentList));
    }

    [HttpGet]
    public IActionResult PaymentList()
    {
        var payments = _db.Payments
            .Include(p => p.Order)
            .OrderByDescending(p => p.PaymentId)
            .ToList();
        return View(payments);
    }

    // หน้าจำลองการชำระเงิน (Demo)
    [HttpGet]
    public IActionResult Pay(int orderId)
    {
        var order = _db.Orders
            .Include(o => o.User)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return RedirectToAction("OrderList", "Order");
        }

        return View(order);
    }

    // กดชำระเงินสำเร็จ (Demo) + โอนเงินเข้า Wallet ร้านค้าผู้ขาย
    [HttpPost]
    public IActionResult Pay(int orderId, string paymentMethod)
    {
        var order = _db.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return RedirectToAction("OrderList", "Order");
        }

        var payment = new Payment
        {
            OrderId = order.OrderId,
            Amount = order.TotalAmount,
            PaymentMethod = paymentMethod ?? order.PaymentMethod ?? "PromptPay",
            Status = "Paid",
            PaidAt = DateTime.Now
        };

        order.Status = "Paid";
        order.PaymentMethod = payment.PaymentMethod;

        _db.Payments.Add(payment);

        // เพิ่มเงินเข้า Wallet ของร้านค้าแต่ละรายการสินค้าที่ขายได้
        foreach (var item in order.OrderItems)
        {
            if (item.Product?.ShopId is int shopId)
            {
                var wallet = _db.ShopWallets.FirstOrDefault(w => w.ShopId == shopId);
                if (wallet == null)
                {
                    wallet = new ShopWallet
                    {
                        ShopId = shopId,
                        Balance = 0,
                        UpdatedAt = DateTime.Now
                    };
                    _db.ShopWallets.Add(wallet);
                    _db.SaveChanges();
                }

                var itemAmount = item.Price * item.Quantity;
                wallet.Balance += itemAmount;
                wallet.UpdatedAt = DateTime.Now;

                _db.WalletTransactions.Add(new WalletTransaction
                {
                    WalletId = wallet.WalletId,
                    OrderId = order.OrderId,
                    Amount = itemAmount,
                    Type = "Income",
                    Description = $"รับเงินจากคำสั่งซื้อ #{order.OrderId} ({item.Product.ProductName} x{item.Quantity})",
                    CreatedAt = DateTime.Now
                });
            }
        }

        _db.Notifications.Add(new Notification
        {
            Title = $"ชำระเงินสำเร็จ #{order.OrderId}",
            Message = $"คำสั่งซื้อ #{order.OrderId} ชำระเงิน {payment.Amount:N2} บาท ผ่าน {payment.PaymentMethod} แล้ว",
            IsRead = false,
            CreatedAt = DateTime.Now
        });

        _db.SaveChanges();

        return RedirectToAction(nameof(PaymentSuccess), new { id = payment.PaymentId });
    }

    [HttpGet]
    public IActionResult PaymentSuccess(int id)
    {
        var payment = _db.Payments
            .Include(p => p.Order)
                .ThenInclude(o => o!.User)
            .FirstOrDefault(p => p.PaymentId == id);
        return View(payment);
    }
}
