using System.ComponentModel.DataAnnotations;

namespace Wishler.ViewModels;

public class RegisterViewModel
{
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Enter your name")]
    [MinLength(3, ErrorMessage = "Your name must contain more than 3 letters")]
    [MaxLength(20, ErrorMessage = "Your name must contain less than 20 letters")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Enter your email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Your password must contain more than 6 letters")]
    [Required(ErrorMessage = "Enter your password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords does not match")]
    public string PasswordConfirm { get; set; }
}