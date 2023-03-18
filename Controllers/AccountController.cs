﻿using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;

    public AccountController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public bool IsValidEmail(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
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
        if (_db.Users.FirstOrDefault(x => x.Email == model.Email) != null)
        {
            ModelState.AddModelError("Email", "There is already a user with such email");
        }
        if (model.Email == null)
        {
            ModelState.AddModelError("Email", "Enter email address");
        } 
        else if (!IsValidEmail(model.Email))
        {
            ModelState.AddModelError("Email", "Invalid email address");
        }
        if (model.Password != model.PasswordConfirm)
        {
            ModelState.AddModelError("Password", "Different passwords provided");
        }
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
        
            var claims = new[] {
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
        if (_db.Users.SingleOrDefault(x => x.Email == model.Email) == null)
        {
            ModelState.AddModelError("Email", "There is no user with such email");
        }
        if (ModelState.IsValid)
        {
            var loginUser = await _db.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(loginUser, loginUser.Password, model.Password);

            if (result == PasswordVerificationResult.Success)
            {
                var userId = _db.Users.Where(x => x.Email == model.Email).ToArray()[0].Id;
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, loginUser.Name),
                    new Claim(ClaimTypes.Email, loginUser.Email)
                };
            
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal));

                return RedirectToAction("Index", "Boards");
            }
            
            ModelState.AddModelError("Password", "Wrong password");
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