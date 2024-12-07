using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers;


[Authorize(Roles = "Director, HR")]
public class VacancyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
