using IdentityProject.Areas.Admin.Models.Email;
using IdentityProject.Entities;
using IdentityProject.Utilities.EmailHandler.Abstract;
using IdentityProject.Utilities.EmailHandler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace IdentityProject.Areas.Admin.Controllers;


[Area("Admin")]
public class EmailController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public EmailController(UserManager<User> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult SendEmail() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult SendEmail(EmailVM model)
    {
        //var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
        

        if (!ModelState.IsValid) return View(model);

        var users = new List<User>();
        if (model.ReceiverType == Constants.ReceiverType.AllUsers)
            users = _userManager.Users.ToList();
        else if (model.ReceiverType == Constants.ReceiverType.Subscribers)
            users = _userManager.Users.Where(u => u.IsSubscribed).ToList();
        else
            return NotFound();

        var url = Url.Action("Index", "Home", new { area = "" }, Request.Scheme);
        var messageContent = $"{model.Content}\n\n{url}";

        foreach (var user in users)
        {
            _emailService.SendMessage(new Message(new List<string> { user.Email }, model.Subject, messageContent));
        }

        TempData["Message"] = $"Emails sent to {users.Count} {((model.ReceiverType == Constants.ReceiverType.AllUsers) ? "all users" : "subscribers")}.";
        return RedirectToAction("SendEmail");

    }
}
