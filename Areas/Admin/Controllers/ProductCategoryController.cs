using IdentityProject.Areas.Admin.Models.ProductCategory;
using IdentityProject.Data;
using IdentityProject.Entities;
using Microsoft.AspNetCore.Mvc;




[Area("Admin")]
public class ProductCategoryController : Controller
{
    private readonly AppDbContext _context;

    public ProductCategoryController(AppDbContext context)
    {
        _context = context;
    }


    #region List

    [HttpGet]
    public IActionResult Index()
    {
        var model = new ProductCategoryIndexVM
        {
            ProductCategories = _context.ProductCategories.ToList()
        };

        return View(model);
    }

    #endregion

    #region Create

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductCategoryCreateVM model)
    {
        if (!ModelState.IsValid) return View();

        var productCategory = _context.ProductCategories.FirstOrDefault(pc => pc.Name.ToLower() == model.Name.ToLower());

        if (productCategory is not null)
        {
            ModelState.AddModelError("Name", "Category has already exists");
            return View();
        }

        productCategory = new ProductCategory
        {
            Name = model.Name,
            CreatedAt = DateTime.Now,
        };

        _context.ProductCategories.Add(productCategory);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Update

    [HttpGet]
    public IActionResult Update(int id)
    {
        var productCategory = _context.ProductCategories.Find(id);
        if (productCategory is null) return NotFound();

        var model = new ProductCategoryUpdateVM
        {
            Name = productCategory.Name,
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Update(int id, ProductCategoryUpdateVM model)
    {
        if (!ModelState.IsValid) return View();

        var productCategory = _context.ProductCategories.Find(id);
        if (productCategory is null) return NotFound();

        var isExist = _context.ProductCategories.Any(pc => pc.Name.ToLower() == model.Name.ToLower() && pc.Id != id);
        if (isExist)
        {
            ModelState.AddModelError("Name", "This name has already exists");
            return View(model);
        }

        if (productCategory.Name != model.Name)
            productCategory.ModifiedAt = DateTime.Now;

        productCategory.Name = model.Name;

        _context.ProductCategories.Update(productCategory);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));

    }

    #endregion

    #region Delete

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var productCategory = _context.ProductCategories.Find(id);
        if (productCategory is null) return NotFound();

        _context.ProductCategories.Remove(productCategory);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion
}
