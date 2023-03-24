using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class Friend
{
    [Key] public int Id { get; set; }

    public string OwnerEmail { get; set; }
    public string FriendEmail { get; set; }
}