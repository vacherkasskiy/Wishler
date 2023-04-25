using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class Group
{
    [Key] public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; } = "";
    public bool IsStarted { get; set; } = false;
}