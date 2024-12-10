using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Constants
{
    public enum ReceiverType
    {
        [Display(Name = ("All Users"))]
        AllUsers,
        Subscribers
    }
}
