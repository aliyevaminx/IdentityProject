using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Models.Account;

public class AccountForgetPasswordVM
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
