using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DShopAPI.Repositories
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

        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public IEnumerable<Product> GetLatestProduct(int count)
        {
            return _dbContext.Products.OrderByDescending(p => p.CreatedAt).Take(count).ToList();
        }

        public IEnumerable<Product> SearchProducts(string query)
        {
            query = query.ToLower();

            return _dbContext.Products
                .Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query) || p.Brand.ToLower().Contains(query))
                .ToList();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products.FirstAsync(p => p.Id == productId);
        }

    }
}
