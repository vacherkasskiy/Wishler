using System.Security.Claims;
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
            GroupsParticipatedAmount = 0,
            FriendsAddedAmount = _db.Friends.Count(x => x.OwnerEmail == userEmail)
        };

        return View(model);
    }

    [HttpPost]
    [Route("/user/profile")]
    public IActionResult Index(EditProfileViewModel newProfile)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var avatarId = int.Parse(User.FindFirstValue(ClaimTypes.UserData));

        if (ModelState.IsValid && newProfile.AvatarId > 0)
        {
            var user = _db.Users.Find(userId);
            user!.Name = newProfile.Name;
            user.AvatarId = newProfile.AvatarId;
            userName = newProfile.Name;
            avatarId = newProfile.AvatarId;
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        var model = new ProfileViewModel
        {
            NewProfile = new EditProfileViewModel
            {
                Name = userName,
                AvatarId = avatarId
            },
            BoardsCreatedAmount = _db.Boards.Count(x => x.UserId == userId),
            GroupsParticipatedAmount = 0,
            FriendsAddedAmount = _db.Friends.Count(x => x.OwnerEmail == userEmail)
        };

        return View(model);
    }
}