using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Areas.Admin.Models.ProductCategory;

public class ProductCategoryCreateVM
{
    [Required(ErrorMessage = "Name required")]
    [MinLength(2, ErrorMessage = "Name must be at least 3 characters")]
    [Display(Name = "Title")]
    public string Name { get; set; }
}
