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
    
    public IActionResult IndexFriendsList(int userId)
    {
        return View(userId);
    }

    [HttpPost]
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

    [HttpPost]
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
    
    [HttpDelete]
    public void CancelFriendRequest(int requestId)
    {
        var request = _db.FriendRequests.Find(requestId);
        if (request != null)
        {
            _db.FriendRequests.Remove(request);
            _db.SaveChanges();
        }
    }

    [HttpDelete]
    public void DeleteFriend(int ownerId, int friendId)
    {
        var owner = _db.Friends.Where(x => x.OwnerId == ownerId && x.FriendId == friendId).ToArray()[0];
        var friend = _db.Friends.Where(x => x.OwnerId == friendId && x.FriendId == x.OwnerId).ToArray()[0];

        _db.Friends.Remove(owner);
        _db.Friends.Remove(friend);

        _db.SaveChanges();
    }
}