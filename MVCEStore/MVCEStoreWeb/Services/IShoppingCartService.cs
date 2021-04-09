using MVCEStoreWeb.Models;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Services
{
    public interface IShoppingCartService
    {
        Task AddToCart(int productId, int quantity = 1);
        Task RemoveFromCart(int productId);
        Task ClearCart();
        ShoppingCart GetCart();


    }
}
