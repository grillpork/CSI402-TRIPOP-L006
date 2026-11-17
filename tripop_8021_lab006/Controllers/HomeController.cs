using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class HomeController : Controller
{
    private readonly CSI402DB _db;

    public HomeController(CSI402DB db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var products = new List<ProductCardViewModel>
        {
            new()
            {
                Name = "Essential Oversized Tee",
                Category = "T-Shirts",
                Price = "฿890",
                ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=900&q=85",
                AccentClass = "bg-stone-100",
                IsNew = true
            },
            new()
            {
                Name = "Relaxed Linen Shirt",
                Category = "Shirts",
                Price = "฿1,290",
                OldPrice = "฿1,590",
                ImageUrl = "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?auto=format&fit=crop&w=900&q=85",
                AccentClass = "bg-slate-100"
            },
            new()
            {
                Name = "Minimal Everyday Bag",
                Category = "Accessories",
                Price = "฿1,490",
                ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?auto=format&fit=crop&w=900&q=85",
                AccentClass = "bg-neutral-100"
            },
            new()
            {
                Name = "Classic Straight Pants",
                Category = "Pants",
                Price = "฿1,190",
                ImageUrl = "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?auto=format&fit=crop&w=900&q=85",
                AccentClass = "bg-zinc-100"
            }
        };

        return View(products);
    }

    public IActionResult Lab9()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Lab9(LabStudent data)
    {
        if (string.IsNullOrEmpty(data.StdPassword))
        {
            ViewBag.Status = "password null";
            return View(data);
        }

        var stdCheck = _db.LabStudents.FirstOrDefault(a => a.StdId == data.StdId);
        if (stdCheck == null)
        {
            var s = new LabStudent
            {
                StdId = data.StdId,
                StdPassword = data.StdPassword,
                StdName = data.StdName,
                StdLastname = data.StdLastname
            };
            _db.LabStudents.Add(s);
            _db.SaveChanges();
            ViewBag.Status = "Saved successfully";
            return View(data);
        }
        else
        {
            ViewBag.Status = "student id already exists";
            return View(data);
        }
    }

    public IActionResult Lab9_1()
    {
        var stdList = _db.LabStudents.ToList();
        return View(stdList);
    }

    [HttpGet]
    public IActionResult Lab9_Edit(string id)
    {
        var student = _db.LabStudents.FirstOrDefault(s => s.StdId == id);
        if (student == null)
        {
            return RedirectToAction(nameof(Lab9_1));
        }
        return View(student);
    }

    [HttpPost]
    public IActionResult Lab9_Edit(LabStudent data)
    {
        var student = _db.LabStudents.FirstOrDefault(s => s.StdId == data.StdId);
        if (student != null)
        {
            student.StdPassword = data.StdPassword;
            student.StdName = data.StdName;
            student.StdLastname = data.StdLastname;
            _db.SaveChanges();
            return RedirectToAction(nameof(Lab9_1));
        }
        ViewBag.Status = "Student not found";
        return View(data);
    }

    public IActionResult Lab9_Delete(string id)
    {
        var student = _db.LabStudents.FirstOrDefault(s => s.StdId == id);
        if (student != null)
        {
            _db.LabStudents.Remove(student);
            _db.SaveChanges();
        }
        return RedirectToAction(nameof(Lab9_1));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Lab2()
    {
        string Name = "Tripop", Lastname = "Saibut", Sec = "006", Lang = "C#", Year = "3";
        ViewBag.User = $"{Name} {Lastname} {Sec} {Lang} {Year}";

        int score1 = 10, score2 = 10, score3 = 10, score4 = 10, score5 = 7, score6 = 10, score7 = 10, score8 = 0, score9 = 0, score10 = 0;
        int sum = score1 + score2 + score3 + score4 + score5 + score6 + score7 + score8 + score9 + score10;

        if (sum >= 80) ViewBag.Result = "A";
        else if (sum >= 75) ViewBag.Result = "B+";
        else if (sum >= 70) ViewBag.Result = "B";
        else if (sum >= 65) ViewBag.Result = "C+";
        else if (sum >= 60) ViewBag.Result = "C";
        else if (sum >= 55) ViewBag.Result = "D+";
        else if (sum >= 50) ViewBag.Result = "D";
        else ViewBag.Result = "F";

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
