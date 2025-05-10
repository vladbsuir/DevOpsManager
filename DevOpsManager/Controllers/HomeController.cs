using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DevOpsManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DevOpsManager.Data;

namespace DevOpsManager.Controllers;

[Authorize]
public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private readonly DevOpsManagerContext _context;

    public HomeController(DevOpsManagerContext context)
    {
        //_logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Users()
    {
        var users = await _context.Users.Include(u => u.Role).ToListAsync();
        return View(users);
    }

    /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }*/
}
