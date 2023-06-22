using DShopAPI.Data;
using DShopAPI.Models;
using DShopAPI.Repository;
using System.Collections.Generic;
using System.Linq;

namespace DShopAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DShopDbContext _dbContext;

        public CategoryRepository(DShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _dbContext.Categories.Find(id);
        }

        public void AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
    }
}
