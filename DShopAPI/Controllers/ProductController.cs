using System.Collections.Generic;
using System.Linq;
using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/categories/{categoryId}/categoryitems/{categoryItemId}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProductsByCategoryItem(int categoryId, int categoryItemId)
        {
            var products = _productRepository.GetProductsByCategoryItem(categoryItemId);

            if (products == null)
            {
                return NotFound();
            }

            // Filter the properties to only include the required fields
            var filteredProducts = products.Select(p => new
            {
                p.Name,
                p.Brand,
                p.Price,
                p.DiscountRate,
                p.ImageUrl,
                p.Rating
            });

            return Ok(filteredProducts);
        }

        [HttpPost]
        public IActionResult CreateProduct(int categoryId, int categoryItemId, Product product)
        {
            // Assign the category item ID and category ID to the product
            product.CategoryItemId = categoryItemId;
            product.CategoryItem.CategoryId = categoryId;

            // Add the product to the repository
            _productRepository.AddProduct(product);

            // Return the created product
            return Ok(product);
        }

        [HttpGet("{productId}")]
        public IActionResult GetProduct(int categoryId, int categoryItemId, int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null || product.CategoryItemId != categoryItemId)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int categoryId, int categoryItemId, int productId, Product updatedProduct)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null || product.CategoryItemId != categoryItemId)
            {
                return NotFound();
            }

            // Update the product properties
            product.Name = updatedProduct.Name;
            product.Brand = updatedProduct.Brand;
            product.Price = updatedProduct.Price;
            product.Description = updatedProduct.Description;
            product.Quantity = updatedProduct.Quantity;
            product.Rating = updatedProduct.Rating;
            product.ImageUrl = updatedProduct.ImageUrl;
            product.DiscountRate = updatedProduct.DiscountRate;

            // Update the product in the repository
            _productRepository.UpdateProduct(product);

            // Return the updated product
            return Ok(product);
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int categoryId, int categoryItemId, int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null || product.CategoryItemId != categoryItemId)
            {
                return NotFound();
            }

            // Delete the product from the repository
            _productRepository.DeleteProduct(product);

            // Return a success message
            return Ok("Product deleted successfully.");
        }

        [HttpGet("filter")]
        public IActionResult FilterProducts(int categoryId, int categoryItemId, decimal minPrice, decimal maxPrice, string size, string color)
        {
            var products = _productRepository.GetProductsByCategoryItem(categoryItemId);

            if (products == null)
            {
                return NotFound();
            }

            // Apply price filter
            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            // Apply size filter
            if (!string.IsNullOrEmpty(size))
            {
                products = products.Where(p => p.Sizes.Any(s => s.Size == size));
            }

            // Apply color filter
            if (!string.IsNullOrEmpty(color))
            {
                products = products.Where(p => p.Colors.Any(c => c.Color == color));
            }

            return Ok(products);
        }
    }
}
