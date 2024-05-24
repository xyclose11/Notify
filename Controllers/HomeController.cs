using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Helpers;
using NoteApp.Models;

namespace NoteApp.Controllers;

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
    
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Delete()
    {
        return View();
    }
    public IActionResult Edit()
    {
        return View();
    }
    public IActionResult Details()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}