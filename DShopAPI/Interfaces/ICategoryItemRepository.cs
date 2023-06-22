using System.Collections.Generic;
using DShopAPI.Models;

namespace DShopAPI.Repository
{
    public interface ICategoryItemRepository
    {
        IEnumerable<CategoryItem> GetByCategoryId(int categoryId);
        CategoryItem GetById(int id);
        CategoryItem Create(CategoryItem categoryItem);
        CategoryItem Update(CategoryItem categoryItem);
        void Delete(CategoryItem categoryItem);
    }
}
