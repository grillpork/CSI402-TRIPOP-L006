using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using final_project.Helpers;
using final_project.Models;

namespace final_project.Controllers;

[Authorize(Roles = "Admin,Seller")]
public class MediaController : Controller
{
    private readonly FinalCSI402DB _db;
    private readonly IWebHostEnvironment _env;

    public MediaController(FinalCSI402DB db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(MediaList));
    }

    [HttpGet]
    public IActionResult MediaList(string? type = null)
    {
        var query = _db.Media.AsQueryable();
        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(m => m.MediaType == type);
        }

        ViewBag.SelectedType = type;
        var list = query.OrderByDescending(m => m.MediaId).ToList();
        return View(list);
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Upload(IFormFile? file, string entityType = "General", int? entityId = null)
    {
        MediaHelper.SaveMedia(file, _env, _db, entityType, entityId);
        return RedirectToAction(nameof(MediaList));
    }

    [HttpPost]
    public IActionResult DeleteMedia(int id)
    {
        var media = _db.Media.Find(id);
        if (media != null)
        {
            var physicalPath = Path.Combine(_env.WebRootPath, media.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            _db.Media.Remove(media);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(MediaList));
    }
}
