using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.Validators;
using Wishler.ViewModels;

namespace Wishler.Controllers;

[Authorize]
public class FriendsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly FriendsValidator _validator;

    public FriendsController(ApplicationDbContext db, FriendsValidator validator)
    {
        _db = db;
        _validator = validator;
    }

    [HttpGet]
    [Route("/user/friends")]
    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var model = new FriendsViewModel
        {
            UserEmail = User.FindFirstValue(ClaimTypes.Email),
            Friends = _db.Friends,
            FriendRequests = _db.FriendRequests,
            FriendRequest = new FriendRequest()
        };

        return View(model);
    }

    [HttpPost]
    [Route("/user/friends")]
    [ValidateAntiForgeryToken]
    public IActionResult Index(FriendRequest friendRequest)
    {
        _validator.ValidateFriendRequest(friendRequest, ModelState);

        if (ModelState.IsValid)
        {
            _db.FriendRequests.Add(friendRequest);
            _db.SaveChanges();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var model = new FriendsViewModel
        {
            UserEmail = User.FindFirstValue(ClaimTypes.Email),
            Friends = _db.Friends,
            FriendRequests = _db.FriendRequests,
            FriendRequest = new FriendRequest()
        };

        return View(model);
    }

    [HttpPost]
    [Route("/AcceptRequest")]
    public IActionResult ApproveFriendRequest(int requestId)
    {
        var friendRequest = _db.FriendRequests.Find(requestId);
        var ownerEmail = friendRequest.SenderEmail;
        var friendEmail = friendRequest.ReceiverEmail;

        var owner = new Friend
        {
            OwnerEmail = ownerEmail,
            FriendEmail = friendEmail
        };

        var friend = new Friend
        {
            OwnerEmail = friendEmail,
            FriendEmail = ownerEmail
        };

        _db.Friends.Add(owner);
        _db.Friends.Add(friend);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpDelete]
    [Route("/DeclineRequest")]
    public void DeclineFriendRequest(int requestId)
    {
        var request = _db.FriendRequests.Find(requestId);
        if (request != null)
        {
            _db.FriendRequests.Remove(request);
            _db.SaveChanges();
        }
    }
}