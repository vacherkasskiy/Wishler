using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wishler.Models;

public class Friend
{
    [Key] public int Id { get; set; }

    [ForeignKey("First")]
    public int FirstId { get; set; }
    [ForeignKey("Second")]
    public int SecondId { get; set; }
    
    public User First { get; set; }
    public User Second { get; set; }
}