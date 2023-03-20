using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Wishler.Data;
using Wishler.Models;

namespace Wishler.Controllers;

[Authorize]
public class FriendController : Controller
{
    private readonly ApplicationDbContext _db;
    
    public FriendController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Index(int userId)
    {
        return View();
    }

    public void SendFriendRequest(int senderId, int receiverId)
    {
        var friendRequest = new FriendRequest
        {
            SenderId = senderId,
            ReceiverId = receiverId
        };

        _db.FriendRequests.Add(friendRequest);
        _db.SaveChanges();
    }

    public void ApproveFriendRequest(int ownerId, int friendId)
    {
        var owner = new Friend
        {
            OwnerId = ownerId,
            FriendId = friendId
        };

        var friend = new Friend
        {
            OwnerId = friendId,
            FriendId = ownerId
        };

        _db.Friends.Add(owner);
        _db.Friends.Add(friend);
        _db.SaveChanges();
    }
}