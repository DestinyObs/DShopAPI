using System.Collections.Generic;
using DShopAPI.Interfaces;
using DShopAPI.Models;

namespace DShopAPI.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private List<Product> _wishlist;

        public WishlistRepository()
        {
            _wishlist = new List<Product>();
        }

        public IEnumerable<Product> GetWishlist()
        {
            return _wishlist;
        }

        public void AddToWishlist(Product product)
        {
            _wishlist.Add(product);
        }

        public void RemoveFromWishlist(int productId)
        {
            var product = _wishlist.Find(p => p.Id == productId);
            if (product != null)
            {
                _wishlist.Remove(product);
            }
        }

        public Product GetProduct(int productId)
        {
            // Here, you would typically retrieve the product from the database based on the productId
            // For demonstration purposes, we'll assume the products are already available in the list
            return _wishlist.Find(p => p.Id == productId);
        }
    }
}

