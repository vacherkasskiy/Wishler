using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class FriendRequest
{
    [Key]
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now; 
}