using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Helpers;
using final_project.Models;

namespace final_project.Controllers;

public class ProductController : Controller
{
    private readonly FinalCSI402DB _db;
    private readonly IWebHostEnvironment _env;

    public ProductController(FinalCSI402DB db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(ProductList));
    }

    [HttpGet]
    public IActionResult ProductList(string? q = null, int? categoryId = null)
    {
        var query = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Shop)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var keyword = q.Trim().ToLower();
            query = query.Where(p =>
                (p.ProductName != null && p.ProductName.ToLower().Contains(keyword)) ||
                (p.Description != null && p.Description.ToLower().Contains(keyword)) ||
                (p.Color != null && p.Color.ToLower().Contains(keyword)) ||
                (p.Size != null && p.Size.ToLower().Contains(keyword)) ||
                (p.Category != null && p.Category.CategoryName != null && p.Category.CategoryName.ToLower().Contains(keyword)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        ViewBag.SearchQuery = q;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.Categories = _db.Categories.ToList();

        return View(query.ToList());
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Shop)
            .FirstOrDefault(p => p.ProductId == id);

        ViewBag.ProductMedia = _db.Media
            .Where(m => m.EntityType == "Product" && m.EntityId == id)
            .OrderBy(m => m.SortOrder)
            .ThenBy(m => m.MediaId)
            .ToList();

        ViewBag.Reviews = _db.Reviews
            .Include(r => r.User)
            .Where(r => r.ProductId == id)
            .OrderByDescending(r => r.ReviewId)
            .ToList();

        ViewBag.RelatedProducts = _db.Products
            .Where(p => p.ProductId != id)
            .OrderByDescending(p => p.ProductId)
            .Take(3)
            .ToList();

        return View(product);
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpGet]
    public IActionResult CreateProduct()
    {
        ViewBag.Categories = _db.Categories.ToList();
        ViewBag.Shops = _db.Shops.ToList();
        return View();
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPost]
    public IActionResult CreateProduct(Product model, List<IFormFile>? imageFiles, IFormFile? videoFile)
    {
        _db.Products.Add(model);
        _db.SaveChanges();

        if (imageFiles != null)
        {
            var sortOrder = 1;
            foreach (var file in imageFiles.Where(f => f.Length > 0))
            {
                var media = MediaHelper.SaveMedia(file, _env, _db, "Product", model.ProductId, sortOrder++);
                if (media != null && string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = media.FilePath;
                }
            }
        }

        if (videoFile != null && videoFile.Length > 0)
        {
            MediaHelper.SaveMedia(videoFile, _env, _db, "Product", model.ProductId, 0);
        }

        _db.SaveChanges();

        return RedirectToAction(nameof(ProductList));
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        ViewBag.Categories = _db.Categories.ToList();
        ViewBag.Shops = _db.Shops.ToList();
        ViewBag.ProductImages = _db.Media
            .Where(m => m.EntityType == "Product" && m.EntityId == id && m.MediaType == "Image")
            .OrderBy(m => m.SortOrder)
            .ThenBy(m => m.MediaId)
            .ToList();
        ViewBag.ProductVideo = _db.Media
            .Where(m => m.EntityType == "Product" && m.EntityId == id && m.MediaType == "Video")
            .OrderByDescending(m => m.MediaId)
            .FirstOrDefault();

        var product = _db.Products.Find(id);
        return View(product);
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPost]
    public IActionResult EditProduct(
        Product model,
        List<IFormFile>? imageFiles,
        IFormFile? videoFile,
        List<int>? mediaIds,
        List<int>? sortOrders)
    {
        if (mediaIds != null && sortOrders != null)
        {
            for (var i = 0; i < Math.Min(mediaIds.Count, sortOrders.Count); i++)
            {
                var existing = _db.Media.Find(mediaIds[i]);
                if (existing != null && existing.EntityType == "Product" && existing.EntityId == model.ProductId)
                {
                    existing.SortOrder = sortOrders[i];
                }
            }
            _db.SaveChanges();
        }

        if (imageFiles != null && imageFiles.Any(f => f.Length > 0))
        {
            var maxOrder = _db.Media
                .Where(m => m.EntityType == "Product" && m.EntityId == model.ProductId && m.MediaType == "Image")
                .Select(m => (int?)m.SortOrder)
                .Max() ?? 0;

            foreach (var file in imageFiles.Where(f => f.Length > 0))
            {
                MediaHelper.SaveMedia(file, _env, _db, "Product", model.ProductId, ++maxOrder);
            }
        }

        if (videoFile != null && videoFile.Length > 0)
        {
            var oldVideos = _db.Media
                .Where(m => m.EntityType == "Product" && m.EntityId == model.ProductId && m.MediaType == "Video")
                .ToList();
            foreach (var oldVid in oldVideos)
            {
                DeletePhysicalFile(oldVid.FilePath);
                _db.Media.Remove(oldVid);
            }
            _db.SaveChanges();

            MediaHelper.SaveMedia(videoFile, _env, _db, "Product", model.ProductId, 0);
        }

        var firstImage = _db.Media
            .Where(m => m.EntityType == "Product" && m.EntityId == model.ProductId && m.MediaType == "Image")
            .OrderBy(m => m.SortOrder)
            .ThenBy(m => m.MediaId)
            .FirstOrDefault();

        model.ImageUrl = firstImage?.FilePath ?? model.ImageUrl;
        _db.Products.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(ProductList));
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPost]
    public IActionResult DeleteProductMedia(int mediaId, int productId)
    {
        var media = _db.Media.Find(mediaId);
        if (media != null && media.EntityType == "Product" && media.EntityId == productId)
        {
            DeletePhysicalFile(media.FilePath);
            _db.Media.Remove(media);
            _db.SaveChanges();
        }

        var product = _db.Products.Find(productId);
        if (product != null)
        {
            var firstImage = _db.Media
                .Where(m => m.EntityType == "Product" && m.EntityId == productId && m.MediaType == "Image")
                .OrderBy(m => m.SortOrder)
                .ThenBy(m => m.MediaId)
                .FirstOrDefault();

            product.ImageUrl = firstImage?.FilePath;
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(EditProduct), new { id = productId });
    }

    [Authorize(Roles = "Admin,Seller")]
    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        var product = _db.Products.Find(id);
        if (product != null)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(ProductList));
    }

    private void DeletePhysicalFile(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        var physicalPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
        if (System.IO.File.Exists(physicalPath))
        {
            System.IO.File.Delete(physicalPath);
        }
    }
}
