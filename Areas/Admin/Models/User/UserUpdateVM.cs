using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Areas.Admin.Models.User;

public class UserUpdateVM
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm password is wrong")]
    public string? ConfirmNewPassword { get; set; }

    public List<SelectListItem>? Roles { get; set; }
    public List<string>? RoleIds { get; set; }
}
