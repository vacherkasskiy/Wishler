using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class Board
{
    [Key] public int Id { get; set; }

    [MaxLength(10, ErrorMessage = "Name must contain less than 10 letters")]
    [Required]
    public string Name { get; set; }

    [Required] public int UserId { get; set; }
    public string BackgroundId { get; set; }
}