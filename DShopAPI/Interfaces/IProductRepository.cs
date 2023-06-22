using System.Collections.Generic;
using System.Linq;
using DShopAPI.Data;
using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductsByCategoryItem(int categoryItemId);
        Product GetProductById(int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }

   
}
