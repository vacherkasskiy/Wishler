using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class Friend
{
    [Key]
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int FriendId { get; set; }
}