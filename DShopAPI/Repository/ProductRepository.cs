using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DShopAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DShopDbContext _dbContext;

        public ProductRepository(DShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Product> GetProductsByCategoryItem(int categoryItemId)
        {
            return _dbContext.Products.Where(p => p.CategoryItemId == categoryItemId).ToList();
        }

        public Product GetProductById(int productId)
        {
            return _dbContext.Products.FirstOrDefault(p => p.Id == productId);
        }

        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }
    }
}
