using IdentityProject.Entities;

namespace IdentityProject.Models.Basket;

public class BasketIndexVM
{

    public BasketIndexVM()
    {
        BasketProducts = new List<BasketProduct>();
    }

    public List<BasketProduct> BasketProducts { get; set; }
}
