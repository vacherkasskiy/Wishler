using System.Net.Mail;
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

    [Route("/user/{userId}/friends")]
    public IActionResult Index(int userId)
    {
        var model = new FriendsViewModel
        {
            UserEmail = _db.Users.Find(userId).Email,
            Friends = _db.Friends,
            FriendRequests = _db.FriendRequests,
            FriendRequest = new FriendRequest()
        };
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SendFriendRequest(FriendRequest friendRequest)
    {
        _validator.ValidateFriendRequest(friendRequest, ModelState);
        
        if (ModelState.IsValid)
        {
            _db.FriendRequests.Add(friendRequest);
            _db.SaveChanges();
        }

        int userId = _db.Users.FirstOrDefault(x => x.Email == friendRequest.SenderEmail).Id;
        return RedirectToAction("Index", new {userId});
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
        
        int userId = _db.Users.FirstOrDefault(x => x.Email == friendRequest.ReceiverEmail).Id;
        return RedirectToAction("Index", new {userId});
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