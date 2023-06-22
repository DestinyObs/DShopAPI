using System.Collections.Generic;
using System.Linq;
using DShopAPI.Data;
using DShopAPI.Models;
using DShopAPI.Repository;

namespace DShopAPI.Repositories
{
    public class CategoryItemRepository : ICategoryItemRepository
    {
        private readonly DShopDbContext _dbContext;

        public CategoryItemRepository(DShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<CategoryItem> GetByCategoryId(int categoryId)
        {
            return _dbContext.CategoryItems.Where(ci => ci.CategoryId == categoryId).ToList();
        }

        public CategoryItem GetById(int id)
        {
            return _dbContext.CategoryItems.FirstOrDefault(ci => ci.Id == id);
        }

        public CategoryItem Create(CategoryItem categoryItem)
        {
            _dbContext.CategoryItems.Add(categoryItem);
            _dbContext.SaveChanges();
            return categoryItem;
        }

        public CategoryItem Update(CategoryItem categoryItem)
        {
            _dbContext.CategoryItems.Update(categoryItem);
            _dbContext.SaveChanges();
            return categoryItem;
        }

        public void Delete(CategoryItem categoryItem)
        {
            _dbContext.CategoryItems.Remove(categoryItem);
            _dbContext.SaveChanges();
        }
    }
}
