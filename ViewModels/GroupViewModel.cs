using Wishler.Models;

namespace Wishler.ViewModels;

public class GroupViewModel
{
    public int GroupId { get; set; }
    public GroupParticipant[] GroupParticipants { get; set; }
    public User[] UsersInGroup { get; set; }
}