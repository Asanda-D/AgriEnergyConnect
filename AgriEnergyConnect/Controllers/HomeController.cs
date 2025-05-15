using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
{
    if (User.Identity?.IsAuthenticated ?? false)
    {
        if (User.IsInRole("Farmer"))
        {
            return RedirectToAction("Products", "Farmer");
        }
        else if (User.IsInRole("Employee"))
        {
            return RedirectToAction("Farmers", "Employee");
        }
    }

    // Not logged in or no matching role — show default home page
    return View();
}

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
