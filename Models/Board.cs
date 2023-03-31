using System.ComponentModel.DataAnnotations;

namespace Wishler.Models;

public class Board
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public int UserId { get; set; }

    public string PictureSource { get; set; }
}