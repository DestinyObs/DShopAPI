using System.Collections.Generic;
using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.Repository;
using DShopAPI.ViewModels.Dtos;
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

            // Map the category items to the response DTO
            var responseDtoList = categoryItems.Select(item => new CategoryItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                CategoryId = item.CategoryId
            }).ToList();

            return Ok(responseDtoList);
        }



        [HttpPost]
        public IActionResult CreateCategoryItem(int categoryId, [FromBody] CreateCategoryItemRequestDto requestDto)
        {
            // Assign the category ID from the URL to the category item
            var categoryItem = new CategoryItem
            {
                Name = requestDto.Name,
                CategoryId = categoryId
            };

            // Add category item to the repository
            _categoryItemRepository.Create(categoryItem);

            // Create the response DTO
            var responseDto = new CreateCategoryItemResponseDto
            {
                CategoryId = categoryItem.CategoryId,
                Name = categoryItem.Name
            };

            // Return the created category item
            return Ok(responseDto);
        }



        [HttpGet("{categoryItemId}")]
        public IActionResult GetCategoryItem(int categoryId, int categoryItemId)
        {
            var categoryItem = _categoryItemRepository.GetById(categoryItemId);

            if (categoryItem == null || categoryItem.CategoryId != categoryId)
            {
                return NotFound();
            }

            // Create the response DTO
            var responseDto = new CategoryItemResponseDto
            {
                Id = categoryItem.Id,
                Name = categoryItem.Name,
                CategoryId = categoryItem.CategoryId
            };

            return Ok(responseDto);
        }


        [HttpPut("{categoryItemId}")]
        public IActionResult UpdateCategoryItem(int categoryId, int categoryItemId, [FromBody] CategoryItemUpdateDto categoryItemUpdateDto)
        {
            var existingCategoryItem = _categoryItemRepository.GetById(categoryItemId);

            if (existingCategoryItem == null || existingCategoryItem.CategoryId != categoryId)
            {
                return NotFound();
            }

            // Update the existing category item with the name from the request body
            existingCategoryItem.Name = categoryItemUpdateDto.Name;

            _categoryItemRepository.Update(existingCategoryItem);

            // Create a response DTO without the category and products properties
            var responseDto = new CategoryItemResponseDto
            {
                Id = existingCategoryItem.Id,
                Name = existingCategoryItem.Name,
                CategoryId = existingCategoryItem.CategoryId
            };

            // Return the updated category item response
            return Ok(responseDto);
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
