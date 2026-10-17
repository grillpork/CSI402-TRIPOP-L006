using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class HomeController : Controller
{
    private readonly Csi402dbContext _db;
    public HomeController(Csi402dbContext db)
    {
        _db = db;
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

        var StdCheck = (from a in _db.LabStudents where a.StdId == data.StdId select a).FirstOrDefault();
        if (StdCheck == null)
        {
            LabStudent s = new();
            //Table2 a = new();
            s.StdId = data.StdId;
            s.StdPassword = data.StdPassword;
            s.StdName = data.StdName;
            s.StdLastname = data.StdLastname;
            //a.ID = data.StdID;
            //_db.Add(a)
            _db.Add(s);
            _db.SaveChanges();
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
        //Read select * from 
        var StdList = (from s in _db.LabStudents select s).ToList();
        //select * from Labstudents
        return View(StdList);
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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Lab2()
    {
        string Name, Lastname, Sec, Lang, Year;

        Name = "Tripop";
        Lastname = "Saibut";
        Sec = "006";
        Lang = "C#";
        Year = "3";

        ViewBag.User = Name + ' ' + Lastname + ' ' + Sec + ' ' + Lang + ' ' + Year;

        int score1 = 10, score2 = 10, score3 = 10, score4 = 10, score5 = 7, score6 = 10, score7 = 10, score8 = 0, score9 = 0, score10 = 0;

        int sum;

        sum = score1 + score2 + score3 + score4 + score5 + score6 + score7 + score8 + score9 + score10;
        if (sum >= 80)
        { ViewBag.Result = "A";
        }
        else if (sum <= 79)
        {
            ViewBag.Result = "B+";
        }
        else if (sum <= 76)
        {
            ViewBag.Result = "B";
        }
        else if (sum <= 69)
        {
            ViewBag.Result = "C+";
        }
        else if (sum <= 66)
        {
            ViewBag.Result = "C";
        }
        else if (sum <= 59)
        {
            ViewBag.Result = "D+";
        }
        else if (sum <= 596)
        {
            ViewBag.Result = "D";
        }
        else
        {
            ViewBag.Result = "F";
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
