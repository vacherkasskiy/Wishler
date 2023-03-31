using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class FriendRequest
{
    [Key] public int Id { get; set; }

    public string SenderEmail { get; set; }

    [Required(ErrorMessage = "Enter email")]
    public string ReceiverEmail { get; set; }

    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}