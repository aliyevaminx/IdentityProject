using IdentityProject.Data;
using IdentityProject.Entities;
using IdentityProject.Models.Basket;
using IdentityProject.Utilities.Stripe;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityProject.Controllers;

public class BasketController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly StripeSettings _stripeSettings;
    private readonly AppDbContext _context;

    public BasketController(UserManager<User> userManager,
                            AppDbContext context,
                            IOptions<StripeSettings> stripeSettings)
    {
        _userManager = userManager;
        _stripeSettings = stripeSettings.Value;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.PublishableKey = _stripeSettings.PublishableKey;

        var checkUser = _userManager.GetUserAsync(User).Result;
        if (checkUser is null) return Unauthorized();

        var user = _userManager.Users.Include(u => u.Basket).FirstOrDefault(u => u.Id == checkUser.Id);
        if (user.Basket is null) return View(new BasketIndexVM());

        var model = new BasketIndexVM
        {
            BasketProducts = _context.BasketProducts.Include(bp => bp.Product).Where(bp => bp.BasketId == user.Basket.Id).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult AddProduct(int productId)
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user is null) return Unauthorized("Product couldn't be added");

        var product = _context.Products.Find(productId);
        if (product is null) return NotFound("Product couldn't be added");

        if (product.StockCount == 0) return BadRequest("Out of stock");

        var basket = _context.Baskets.FirstOrDefault(b => b.UserId == user.Id);
        if (basket is null)
        {
            basket = new Basket
            {
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };

            _context.Baskets.Add(basket);
        }

        var basketProduct = _context.BasketProducts.FirstOrDefault(bp => bp.ProductId == productId && bp.Basket.UserId == user.Id);

        if (basketProduct is null)
        {
            basketProduct = new BasketProduct
            {
                Basket = basket,
                ProductId = productId,
                Count = 1,
                CreatedAt = DateTime.Now
            };

            _context.BasketProducts.Add(basketProduct);
        }
        else
        {
            if (basketProduct.Count == product.StockCount)
                return BadRequest("Out of stock");

            basketProduct.Count++;
            _context.BasketProducts.Update(basketProduct);  
        }

        
        _context.SaveChanges();

        return Ok("The product was successfully added to the basket");
    }

    [HttpPost]
    public IActionResult IncreaseCount(int basketProductId)
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user is null) return Unauthorized();

        var basketProduct = _context.BasketProducts.Include(bp => bp.Basket).FirstOrDefault(bp => bp.Id == basketProductId);
        if (basketProduct is null) return BadRequest("Product not found");

        if (basketProduct.Basket.UserId != user.Id) return BadRequest("It was not possible to increase the number of products");

        var product = _context.Products.Find(basketProduct.ProductId);
        if (product is null) return NotFound("Product not found");

        if (basketProduct.Count == product.StockCount) return BadRequest("Out of stock");

        basketProduct.Count++;

        _context.BasketProducts.Update(basketProduct);
        _context.SaveChanges();

        return Ok(new
        {
            count = basketProduct.Count,
            totalAmountForProduct = basketProduct.Count * product.Price,
            totalAmount = _context.BasketProducts.Sum(bp => bp.Count * bp.Product.Price)
        });

    }


    [HttpPost]
    public IActionResult DecreaseCount(int basketProductId)
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user is null) return Unauthorized();

        var basketProduct = _context.BasketProducts.Include(bp => bp.Basket).FirstOrDefault(bp => bp.Id == basketProductId);
        if (basketProduct is null) return BadRequest("Product not found");

        if (basketProduct.Basket.UserId != user.Id) return BadRequest("It was not possible to increase the number of products");

        var product = _context.Products.Find(basketProduct.ProductId);
        if (product is null) return NotFound("Product not found");

        if (basketProduct.Count == 1) return BadRequest("There should be at least one product");

        basketProduct.Count--;

        _context.BasketProducts.Update(basketProduct);
        _context.SaveChanges();

        return Ok(new
        {
            count = basketProduct.Count,
            totalAmountForProduct = basketProduct.Count * product.Price,
            totalAmount = _context.BasketProducts.Sum(bp => bp.Count * bp.Product.Price)
        });

    }

    [HttpPost]
    public IActionResult Delete(int basketProductId)
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user is null) return Unauthorized();

        var basketProduct = _context.BasketProducts.Include(bp => bp.Basket).FirstOrDefault(bp => bp.Id == basketProductId);
        if (basketProduct is null) return NotFound("Product not found");

        if (basketProduct.Basket.UserId != user.Id)
            return BadRequest("The product could not be deleted");

        _context.BasketProducts.Remove(basketProduct);
        _context.SaveChanges();

        return Ok(new
        {
            totalAmount = _context.BasketProducts.Sum(bp => bp.Count * bp.Product.Price)
        });
    }
}
