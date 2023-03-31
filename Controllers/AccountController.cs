using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.Validators;
using Wishler.ViewModels;

namespace Wishler.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly AccountValidator _validator;

    public AccountController(ApplicationDbContext db, AccountValidator validator)
    {
        _db = db;
        _validator = validator;
    }

    [Route("/register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        _validator.ValidateRegister(model, ModelState);
        if (ModelState.IsValid)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Boards");
        }

        return View();
    }

    [Route("/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        _validator.ValidateLogin(model, ModelState);
        if (ModelState.IsValid)
        {
            var loginUser = _db.Users.SingleOrDefault(u => u.Email == model.Email);

            var userId = _db.Users.Where(x => x.Email == model.Email).ToArray()[0].Id;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, loginUser.Name),
                new Claim(ClaimTypes.Email, loginUser.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(principal));

            return RedirectToAction("Index", "Boards");
        }

        return View();
    }

    [Route("/account/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}