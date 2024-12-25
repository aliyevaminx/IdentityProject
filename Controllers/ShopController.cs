using IdentityProject.Data;
using IdentityProject.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



public class ShopController : Controller
{
    private readonly AppDbContext _context;

    public ShopController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var model = new ProductIndexVM
        {
            ProductCategories = _context.ProductCategories.Include(p => p.Products).ToList(),
            Products = _context.Products.ToList()
        };

        return View(model);
    }


    [HttpGet]
    public IActionResult ShowProducts(int categoryId)
    {
        var products = _context.Products.ToList();

        if (categoryId > 0)
        {
            products = _context.Products.Where(p => p.ProductCategoryId == categoryId).ToList();
        }

        return PartialView("_ProductPartial", products);
    }

    public IActionResult Details(int id)
    {
        return View();
    }
}
