using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;

namespace DShopAPI.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DShopDbContext _dbContext;

        public CartItemRepository(DShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<CartItem> GetCartItemById(int cartItemId)
        {
            return Task.FromResult(_dbContext.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId));
        }

        public async Task AddToCart(CartItem cartItem)
        {
            _dbContext.CartItems.Add(cartItem);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveFromCart(CartItem cartItem)
        {
            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
        }


        public Task<CartItem> GetCartByUserId(int userId)
        {
            return Task.FromResult(_dbContext.CartItems.FirstOrDefault(ci => ci.UserId == userId));
        }

        // Implement other methods as needed
    }
}
