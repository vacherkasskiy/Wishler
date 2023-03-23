using Wishler.Models;

namespace Wishler.ViewModels;

public class FriendsViewModel
{
    public string UserEmail { get; set; }
    public IEnumerable<Friend> Friends { get; set; }
    public IEnumerable<FriendRequest> FriendRequests { get; set; }
    public FriendRequest FriendRequest { get; set; }
}