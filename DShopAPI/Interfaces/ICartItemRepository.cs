using System.Threading.Tasks;
using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemById(int cartItemId);
        Task AddToCart(CartItem cartItem);
        Task RemoveFromCart(CartItem cartItem);
        Task<CartItem> GetCartByUserId(int userId); // Add this line
        // Add more methods as needed
    }
}
