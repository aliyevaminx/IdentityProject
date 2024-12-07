using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers;

[Authorize(Roles = "Director, Manager")]
public class AboutController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

}
