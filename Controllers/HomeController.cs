using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bilgi_Güvenliği_ve_Kriptoloji.Models;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewBag.Title = "Kriptografi Uygulaması Hakkında";
        ViewBag.Message = "Bu uygulama, kriptografi algoritmaları hakkında bilgi vermek ve pratik yapmak için tasarlanmıştır.";

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
