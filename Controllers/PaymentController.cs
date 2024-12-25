using IdentityProject.Constants;
using IdentityProject.Data;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityProject.Controllers;

[Authorize]
public class PaymentController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public PaymentController(AppDbContext context,
                             UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public IActionResult Pay()
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user is null) return Unauthorized();

        user = _context.Users.Include(u => u.Basket).ThenInclude(b => b.BasketProducts).FirstOrDefault(u => u.Id == user.Id);

        var order = new Order
        {
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.Now,
            UserId = user.Id
        };

        _context.Orders.Add(order);

        foreach (var basketProduct in user.Basket.BasketProducts)
        {
            var orderProduct = new OrderProduct
            {
                Order = order
            };
        }

        _context.SaveChanges();
        return View();
    }

    public IActionResult Success()
    {
        return Ok();
    }

    public IActionResult Fail()
    {
        return Ok();
    }
}
