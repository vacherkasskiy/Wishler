using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Wishler.Models;

namespace Wishler.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    [Route("/login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [Route("/register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/register")]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Index", "Boards");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}