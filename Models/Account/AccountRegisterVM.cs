using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Models.Account;

public class AccountRegisterVM
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }

    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Confirm password is wrong")]
    public string ConfirmPassword { get; set; }
}
