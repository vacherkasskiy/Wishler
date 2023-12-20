using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class GroupOwner
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Owner")]
    public int OwnerId { get; set; }
    [ForeignKey("Group")]
    public int GroupId { get; set; }
    
    public User Owner { get; set; }
    public Group Group { get; set; }
}