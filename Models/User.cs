using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class User
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [EmailAddress] [Required] public string Email { get; set; }

    public string Password { get; set; }

    public int AvatarId { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}