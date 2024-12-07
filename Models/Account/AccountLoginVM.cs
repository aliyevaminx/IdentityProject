using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Models.Account;

public class AccountLoginVM
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string? ReturnUrl { get; set; }
}
