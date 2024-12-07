using IdentityProject.Areas.Admin.Models.Account;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManage)
    {
        _userManager = userManager;
        _signInManager = signInManage;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(AccountLoginVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user =  _userManager.FindByEmailAsync(model.Email).Result;  
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }

        if(!_userManager.IsInRoleAsync(user, "Admin").Result)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }

        var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("index", "dashboard");
    } 
}
