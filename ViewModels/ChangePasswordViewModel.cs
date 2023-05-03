using System.ComponentModel.DataAnnotations;

namespace Wishler.ViewModels;

public class ChangePasswordViewModel
{
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Enter your password")]
    public string OldPassword { get; set; }
    
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Your new password must contain more than 6 letters")]
    [Required(ErrorMessage = "Enter new password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm your new password")]
    [Compare("NewPassword", ErrorMessage = "Passwords does not match")]
    public string ConfirmNewPassword { get; set; }
}