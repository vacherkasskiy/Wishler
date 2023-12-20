using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class FriendRequest
{
    [Key] public int Id { get; set; }
    [ForeignKey("Sender")]
    public int SenderId { get; set; }
    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    
    public User Sender { get; set; }
    public User Receiver { get; set; }
}