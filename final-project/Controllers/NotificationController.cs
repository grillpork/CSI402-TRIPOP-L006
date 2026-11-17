using Microsoft.AspNetCore.Mvc;
using final_project.Models;

namespace final_project.Controllers;

public class NotificationController : Controller
{
    private readonly FinalCSI402DB _db;

    public NotificationController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(NotificationList));
    }

    [HttpGet]
    public IActionResult NotificationList()
    {
        var list = _db.Notifications
            .OrderByDescending(n => n.NotificationId)
            .ToList();
        return View(list);
    }

    [HttpPost]
    public IActionResult MarkAsRead(int id)
    {
        var item = _db.Notifications.Find(id);
        if (item != null)
        {
            item.IsRead = true;
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(NotificationList));
    }

    [HttpPost]
    public IActionResult MarkAllAsRead()
    {
        var unread = _db.Notifications.Where(n => !n.IsRead).ToList();
        foreach (var item in unread)
        {
            item.IsRead = true;
        }
        _db.SaveChanges();

        return RedirectToAction(nameof(NotificationList));
    }

    [HttpPost]
    public IActionResult DeleteNotification(int id)
    {
        var item = _db.Notifications.Find(id);
        if (item != null)
        {
            _db.Notifications.Remove(item);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(NotificationList));
    }
}
