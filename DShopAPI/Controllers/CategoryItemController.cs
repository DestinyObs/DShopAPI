using System.Collections.Generic;
using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/categories/{categoryId}/[controller]")]
    public class CategoryItemController : ControllerBase
    {
        private readonly ICategoryItemRepository _categoryItemRepository;

        public CategoryItemController(ICategoryItemRepository categoryItemRepository)
        {
            _categoryItemRepository = categoryItemRepository;
        }


        [HttpGet]
        public IActionResult GetCategoryItems(int categoryId)
        {
            var categoryItems = _categoryItemRepository.GetByCategoryId(categoryId);

            if (categoryItems == null)
            {
                return NotFound();
            }

            return Ok(categoryItems);
        }


        [HttpPost]
        public IActionResult CreateCategoryItem(int categoryId, CategoryItem categoryItem)
        {
            // Assign the category ID to the category item
            categoryItem.CategoryId = categoryId;

            // Add category item to the repository
            _categoryItemRepository.Create(categoryItem);

            // Return the created category item
            return Ok(categoryItem);
        }

        [HttpGet("{categoryItemId}")]
        public IActionResult GetCategoryItem(int categoryId, int categoryItemId)
        {
            var categoryItem = _categoryItemRepository.GetById(categoryItemId);

            if (categoryItem == null || categoryItem.CategoryId != categoryId)
            {
                return NotFound();
            }

            return Ok(categoryItem);
        }

        [HttpPut("{categoryItemId}")]
        public IActionResult UpdateCategoryItem(int categoryId, int categoryItemId, CategoryItem categoryItem)
        {
            var existingCategoryItem = _categoryItemRepository.GetById(categoryItemId);

            if (existingCategoryItem == null || existingCategoryItem.CategoryId != categoryId)
            {
                return NotFound();
            }

            // Update the existing category item
            existingCategoryItem.Name = categoryItem.Name;

            _categoryItemRepository.Update(existingCategoryItem);

            // Return the updated category item
            return Ok(existingCategoryItem);
        }

        [HttpDelete("{categoryItemId}")]
        public IActionResult DeleteCategoryItem(int categoryId, int categoryItemId)
        {
            var existingCategoryItem = _categoryItemRepository.GetById(categoryItemId);

            if (existingCategoryItem == null || existingCategoryItem.CategoryId != categoryId)
            {
                return NotFound();
            }

            // Delete the category item
            _categoryItemRepository.Delete(existingCategoryItem);

            // Return a success message
            return Ok("Category item deleted successfully.");
        }
    }
}
