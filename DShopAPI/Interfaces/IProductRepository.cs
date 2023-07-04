using System.Collections.Generic;
using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductsByCategoryItem(int categoryItemId);
        Task<Product> GetProductByIdAsync(int productId);
        Product GetProductById(int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetLatestProduct(int count);
        IEnumerable<Product> SearchProducts(string query);
    }
}
