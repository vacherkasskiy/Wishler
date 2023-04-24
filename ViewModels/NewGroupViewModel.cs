using System.ComponentModel.DataAnnotations;
using Wishler.Models;

namespace Wishler.ViewModels;

public class NewGroupViewModel
{
    [Required]
    [MinLength(5)]
    [MaxLength(20)]
    public string Name { get; set; }
    public string Members { get; set; }
}