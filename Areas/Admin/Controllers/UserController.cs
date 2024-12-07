using IdentityProject.Areas.Admin.Models.User;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityProject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var users = new List<UserVM>();
        foreach (var user in _userManager.Users.ToList())
        {
            if (!_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                users.Add(new UserVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    City = user.City,
                    Country = user.Country,
                    PhoneNumber = user.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(user).Result.ToList()
                });
            }
        }

        var model = new UserIndexVM
        {
            Users = users
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Details(string id)
    {
        var user = _userManager.FindByIdAsync(id).Result;
        if (user is null) return NotFound();

        var model = new UserDetailsVM
        {
            Email = user.Email,
            City = user.City,
            Country = user.Country,
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new UserCreateVM
        {
            Roles = _roleManager.Roles.Where(r => r.Name != "Admin").Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(UserCreateVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new User
        {
            Email = model.Email,
            UserName = model.Email,
            City = model.City,
            Country = model.Country,
            PhoneNumber = model.PhoneNumber
        };

        var result = _userManager.CreateAsync(user, model.Password).Result;
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        foreach (var roleId in model.RoleIds)
        {
            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role is null)
            {
                ModelState.AddModelError("RoleIds", "Role isn't available");
                return View(model);
            }

            var addToRoleResult = _userManager.AddToRoleAsync(user, role.Name).Result;
            if (!addToRoleResult.Succeeded)
            {
                ModelState.AddModelError("RoleIds", "It was not possible to add a role to the user");
                return View(model);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        var user = _userManager.FindByIdAsync(id).Result;
        if (user is null) return NotFound();

        var result = _userManager.DeleteAsync(user).Result;
        if(!result.Succeeded) return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
