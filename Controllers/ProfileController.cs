using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.ViewModels;

namespace Wishler.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProfileController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("/user/profile")]
    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var model = new ProfileViewModel
        {
            NewProfile = new EditProfileViewModel
            {
                Name = User.FindFirstValue(ClaimTypes.Name),
                AvatarId = int.Parse(User.FindFirstValue(ClaimTypes.UserData))
            },
            BoardsCreatedAmount = _db.Boards.Count(x => x.UserId == userId),
            GroupsParticipatedAmount = _db.GroupParticipants.Count(x => x.UserId == userId),
            FriendsAddedAmount = _db.Friends.Count(x => x.OwnerEmail == userEmail)
        };

        return View(model);
    }

    [HttpPost]
    [Route("/user/profile")]
    public async Task<IActionResult> Index(EditProfileViewModel newProfile)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var avatarId = int.Parse(User.FindFirstValue(ClaimTypes.UserData));

        if (ModelState.IsValid && newProfile.AvatarId > 0)
        {
            var user = await _db.Users.FindAsync(userId);
            user!.Name = newProfile.Name;
            user.AvatarId = newProfile.AvatarId;
            userName = newProfile.Name;
            avatarId = newProfile.AvatarId;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.UserData, user.AvatarId.ToString()) // avatar id
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(principal));
        }

        var model = new ProfileViewModel
        {
            NewProfile = new EditProfileViewModel
            {
                Name = userName,
                AvatarId = avatarId
            },
            BoardsCreatedAmount = _db.Boards.Count(x => x.UserId == userId),
            GroupsParticipatedAmount = _db.GroupParticipants.Count(x => x.UserId == userId),
            FriendsAddedAmount = _db.Friends.Count(x => x.OwnerEmail == userEmail)
        };

        return View(model);
    }
}