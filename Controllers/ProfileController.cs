using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.ViewModels;

namespace Wishler.Controllers;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProfileController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [HttpGet]
    [Authorize]
    [Route("/user/{userId}/profile")]
    public IActionResult Index(int userId)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        
        var model = new ProfileViewModel
        {
            BoardsCreatedAmount = _db.Boards.Count(x => x.UserId == userId),
            GroupsParticipatedAmount = 0,
            FriendsAddedAmount = _db.Friends.Count(x => x.OwnerEmail == userEmail)
        };
        
        return View(model);
    }
}