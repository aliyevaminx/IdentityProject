using IdentityProject.Constants;
using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Areas.Admin.Models.Email;

public class EmailVM
{
    public string Subject { get; set; }
    public string Content { get; set; }

    [Required]
    public ReceiverType ReceiverType { get; set; }
}
