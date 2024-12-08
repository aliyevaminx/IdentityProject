using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Models.Account;

public class AccountResetPasswordVM
{
    [Required(ErrorMessage = "New password is required")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm new password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm password is wrong")]
    public string ConfirmNewPassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }
}
