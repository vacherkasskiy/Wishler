using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class GroupParticipant
{
    [Key] public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    [ForeignKey("Group")]
    public int GroupId { get; set; }
    
    public User User { get; set; }
    public Group Group { get; set; }
}