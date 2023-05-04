using Wishler.Models;

namespace Wishler.ViewModels;

public class GroupViewModel
{
    public bool IsStarted { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; } = "";
    public GroupParticipant[] GroupParticipants { get; set; }
    public User[] UsersInGroup { get; set; }
    public User[] PossibleMembers { get; set; }
}