using System.Collections.Generic;
using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface IWishlistRepository
    {
        IEnumerable<Product> GetWishlist();
        void AddToWishlist(Product product);
        void RemoveFromWishlist(int productId);
        Product GetProduct(int productId);
    }
}
