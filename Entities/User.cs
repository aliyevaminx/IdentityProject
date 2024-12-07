using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Entities
{
    public class User : IdentityUser
    {
        public string City { get; set; }
        public string Country { get; set; }
    }
}
