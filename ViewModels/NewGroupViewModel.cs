using System.ComponentModel.DataAnnotations;

namespace Wishler.ViewModels;

public class NewGroupViewModel
{
    [Required]
    [StringLength(20, MinimumLength = 4)]
    public string Name { get; set; }

    public string Members { get; set; }
}