using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Validators;

public class AccountValidator
{
    private readonly ApplicationDbContext _db;

    public AccountValidator(ApplicationDbContext db)
    {
        _db = db;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var m = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public void ValidateRegister(RegisterViewModel model, ModelStateDictionary modelState)
    {
        if (_db.Users.FirstOrDefault(x => x.Email == model.Email) != null)
            modelState.AddModelError("Email", "There is already a user with such email");
        if (model.Email == null)
            modelState.AddModelError("Email", "Enter email address");
        else if (!IsValidEmail(model.Email)) modelState.AddModelError("Email", "Invalid email address");
        if (model.Password != model.PasswordConfirm)
            modelState.AddModelError("Password", "Different passwords provided");
    }

    public void ValidateLogin(LoginViewModel model, ModelStateDictionary modelState)
    {
        var loginUser = _db.Users.SingleOrDefault(u => u.Email == model.Email);

        if (loginUser == null)
        {
            modelState.AddModelError("Email", "There is no user with such email");
        }
        else
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(loginUser, loginUser.Password, model.Password);

            if (result != PasswordVerificationResult.Success) modelState.AddModelError("Password", "Wrong password");
        }
    }
}