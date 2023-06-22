using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.Repository;
using DShopAPI.ViewModels.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryRepository.GetAllCategories();

            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Ok(categoryDtos);
        }


        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("Add-Category")]
        public IActionResult AddCategory(CategoryNameDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            _categoryRepository.AddCategory(category);

            var categoryResponseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok($"Category {categoryResponseDto.Name} Has Successfully Been Created As Id {categoryResponseDto.Id} ");
        }


        [HttpPut("categories/{id}")]
        public IActionResult UpdateCategory(int id, CategoryNameDto categoryDto)
        {
            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.Name = categoryDto.Name;
            _categoryRepository.UpdateCategory(existingCategory);
            return Ok("Category Successfully Updated");
        }


        [HttpDelete("Delete-Category/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            _categoryRepository.DeleteCategory(category);
            return Ok();
        }
    }
}
