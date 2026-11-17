using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Controllers;

public class ReviewController : Controller
{
    private readonly FinalCSI402DB _db;

    public ReviewController(FinalCSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return RedirectToAction(nameof(ReviewList));
    }

    [HttpGet]
    public IActionResult ReviewList()
    {
        var reviews = _db.Reviews
            .Include(r => r.Product)
            .Include(r => r.User)
            .OrderByDescending(r => r.ReviewId)
            .ToList();
        return View(reviews);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var review = _db.Reviews
            .Include(r => r.Product)
            .Include(r => r.User)
            .FirstOrDefault(r => r.ReviewId == id);
        return View(review);
    }

    [HttpGet]
    public IActionResult CreateReview()
    {
        ViewBag.Products = _db.Products.ToList();
        ViewBag.Users = _db.Users.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult CreateReview(Review model)
    {
        model.CreatedAt = DateTime.Now;
        _db.Reviews.Add(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(ReviewList));
    }

    [HttpGet]
    public IActionResult EditReview(int id)
    {
        ViewBag.Products = _db.Products.ToList();
        ViewBag.Users = _db.Users.ToList();
        var review = _db.Reviews.Find(id);
        return View(review);
    }

    [HttpPost]
    public IActionResult EditReview(Review model)
    {
        _db.Reviews.Update(model);
        _db.SaveChanges();

        return RedirectToAction(nameof(ReviewList));
    }

    [HttpPost]
    public IActionResult DeleteReview(int id)
    {
        var review = _db.Reviews.Find(id);
        if (review != null)
        {
            _db.Reviews.Remove(review);
            _db.SaveChanges();
        }

        return RedirectToAction(nameof(ReviewList));
    }
}
