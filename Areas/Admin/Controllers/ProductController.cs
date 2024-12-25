using IdentityProject.Areas.Admin.Models.Product;
using IdentityProject.Data;
using IdentityProject.Entities;
using IdentityProject.Utilities.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


[Area("Admin")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;

    public ProductController(AppDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    #region List

    public IActionResult Index()
    {
        var model = new ProductIndexVM
        {
            Products = _context.Products.Include(p => p.ProductCategory).ToList()
        };

        return View(model);
    }

    #endregion

    #region Create

    [HttpGet]
    public IActionResult Create()
    {
        var model = new ProductCreateVM
        {
            ProductCategories = _context.ProductCategories.Select(pc => new SelectListItem
            {
                Text = pc.Name,
                Value = pc.Id.ToString()
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(ProductCreateVM model)
    {

        model.ProductCategories = _context.ProductCategories.Select(pc => new SelectListItem
        {
            Text = pc.Name,
            Value = pc.Id.ToString()
        }).ToList();

        if (!ModelState.IsValid) return View(model);

        var product = _context.Products.FirstOrDefault(p => p.Title == model.Title);
        if (product is not null)
        {
            ModelState.AddModelError("Product", "This product has already exists");
            return View(model);
        }

        var productCategory = _context.ProductCategories.Find(model.ProductCategoryId);
        if (productCategory is null)
        {
            ModelState.AddModelError("Category", "This category is not available");
            return View(model);
        }

        if (!_fileService.IsImage(model.Photo.ContentType))
            ModelState.AddModelError("Photo", "The image is not in the correct format");

        if (!_fileService.IsTrueSize(model.Photo.Length))
            ModelState.AddModelError("Photo", "Length must be less than 500 kb");

        var photoName = _fileService.Upload(model.Photo, "main-assets/img");

        product = new Product
        {
            Title = model.Title,
            Size = model.Size,
            Price = model.Price,
            StockCount = model.StockCount,
            Photo = photoName,
            ProductCategoryId = productCategory.Id,
            CreatedAt = DateTime.Now,
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Update

    [HttpGet]
    public IActionResult Update(int id)
    {
        var product = _context.Products.Find(id);
        if (product is null) return NotFound();

        var model = new ProductUpdateVM
        {
            Title = product.Title,
            Size = product.Size,
            Price = product.Price,
            StockCount = product.StockCount,
            PhotoName = product.Photo,
            ProductCategoryId = product.ProductCategoryId,
            ProductCategories = _context.ProductCategories.Select(pc => new SelectListItem
            {
                Text = pc.Name,
                Value = pc.Id.ToString()
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Update(int id, ProductUpdateVM model)
    {
        model.ProductCategories = _context.ProductCategories.Select(pc => new SelectListItem
        {
            Text = pc.Name,
            Value = pc.Id.ToString()
        }).ToList();

        var product = _context.Products.Find(id);
        if (product is null) return NotFound();

        var isExist = _context.Products.Any(p => p.Title.ToLower() == model.Title.ToLower() && p.Id != product.Id);
        if (isExist)
        {
            ModelState.AddModelError("Title", "Product has already exists");
            return View(model);
        }

        var productCategory = _context.ProductCategories.Find(model.ProductCategoryId);
        if (productCategory is null) return NotFound();

        product.Title = model.Title;
        product.Size = model.Size;
        product.Price = model.Price;
        product.StockCount = model.StockCount;
        product.ProductCategoryId = productCategory.Id;
        product.ModifiedAt = DateTime.Now;

        if (model.Photo is not null)
        {
            if (!_fileService.IsImage(model.Photo.ContentType))
                ModelState.AddModelError("Photo", "The image is not in the correct format");

            if (!_fileService.IsTrueSize(model.Photo.Length))
                ModelState.AddModelError("Photo", "Length must be less than 500 kb");

            _fileService.Delete("assets/img", product.Photo);
            product.Photo = _fileService.Upload(model.Photo, "assets/img");
        }

        _context.Products.Update(product);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));

    }

    #endregion

    #region Delete
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product is null) return NotFound();

        _context.Products.Remove(product);
        _context.SaveChanges();

        _fileService.Delete("assets/img", product.Photo);

        return RedirectToAction(nameof(Index));
    }
    #endregion
}
