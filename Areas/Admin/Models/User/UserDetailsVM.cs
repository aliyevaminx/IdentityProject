using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityProject.Areas.Admin.Models.User;

public class UserDetailsVM
{
    public string Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public List<string>? Roles { get; set; }
}
