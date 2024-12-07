using IdentityProject.Entities;
using IdentityProject.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers;

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
    public IActionResult Register()
    {
        return View(); 
    }

    [HttpPost]
    public IActionResult Register(AccountRegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new User
        {
            Email = model.Email,
            UserName = model.Email,
            Country = model.Country,
            City = model.City,
            PhoneNumber = model.PhoneNumber
        };

        var result = _userManager.CreateAsync(user, model.Password).Result;
        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        return RedirectToAction(nameof(Login));
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

        var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");
            return View(model);
        }

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("index", "home");
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
