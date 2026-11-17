using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using tripop_8021_lab006.Models;

namespace tripop_8021_lab006.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
