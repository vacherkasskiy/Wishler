using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using Wishler.Models;

namespace Wishler.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;

    public AccountController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [Route("/register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register(User user)
    {
        // add validation here
        var passwordHasher = new PasswordHasher<User>();
        user.Password = passwordHasher.HashPassword(user, user.Password);
        
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Boards");
    }
    
    [Route("/login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login(User user)
    {
        var loginUser = await _db.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
        if (loginUser == null)
        {
            // add validation here
            ModelState.AddModelError("Email", "Invalid email address");
            return View();
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(loginUser, loginUser.Password, user.Password);

        if (result == PasswordVerificationResult.Success)
        {
            var userId = _db.Users.Where(x => x.Email == user.Email).ToArray()[0].id;
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal));

            return RedirectToAction("Index", "Boards");
        }
        
        // add validation here
        ModelState.AddModelError("Password", "Invalid password");
        return View();
    }

}