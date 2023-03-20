using Wishler.Models;

namespace Wishler.ViewModels;

public class FriendsViewModel
{
    public int UserId { get; set; }
    public IEnumerable<Friend> Friends { get; set; }
    public IEnumerable<FriendRequest> FriendRequests { get; set; }
}