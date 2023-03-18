using System.ComponentModel.DataAnnotations;

namespace Wishler.ViewModels;

public class LoginViewModel
{
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Enter your email")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Enter your password")]
    public string Password { get; set; }
}