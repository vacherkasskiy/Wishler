using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishler.Data;
using Wishler.Models;
using Wishler.ViewModels;

namespace Wishler.Controllers;

[Authorize]
public class FriendsController : Controller
{
    private readonly ApplicationDbContext _db;

    public FriendsController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    private bool IsValidEmail(string email)
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
        if (friendRequest.ReceiverEmail == null || !IsValidEmail(friendRequest.ReceiverEmail))
        {
            ModelState.AddModelError("FriendRequest.ReceiverEmail", "Invalid email");
        }
        if (_db.Users.FirstOrDefault(x => x.Email == friendRequest.ReceiverEmail) == null)
        {
            ModelState.AddModelError("FriendRequest.ReceiverEmail", "There is no such user");
        }
        if (friendRequest.ReceiverEmail == friendRequest.SenderEmail)
        {
            ModelState.AddModelError("FriendRequest.ReceiverEmail", "This is you");
        }
        if (_db.FriendRequests.FirstOrDefault(x => x.SenderEmail == friendRequest.SenderEmail
                                                   && x.ReceiverEmail == friendRequest.ReceiverEmail) != null)
        {
            ModelState.AddModelError("FriendRequest.ReceiverEmail", "You already sent this request");
        }
        if (_db.FriendRequests.FirstOrDefault(x => x.SenderEmail == friendRequest.ReceiverEmail
                                                   && x.ReceiverEmail == friendRequest.SenderEmail) != null)
        {
            ModelState.AddModelError("FriendRequest.ReceiverEmail", "This user already sent you request");
        }
        if (_db.Friends.FirstOrDefault(x => x.OwnerEmail == friendRequest.SenderEmail
                                            && x.FriendEmail == friendRequest.ReceiverEmail) != null)
        {
            ModelState.AddModelError("FriendRequest.ReceiverEmail", "This user is already your friend");
        }
        
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