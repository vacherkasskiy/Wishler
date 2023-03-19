using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class FriendRequest
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now; 
}