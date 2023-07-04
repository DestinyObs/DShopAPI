using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.ViewModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/categoryitems/{categoryItemId}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProductsByCategoryItem(int categoryItemId)
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
        public IActionResult CreateProduct(int categoryItemId, AddProductDto addProductDto)
        {
            // Map the DTO to the Product entity
            var product = new Product
            {
                Name = addProductDto.Name,
                Price = addProductDto.Price,
                Description = addProductDto.Description,
                Quantity = addProductDto.Quantity,
                Brand = addProductDto.Brand,
                ImageUrl = addProductDto.ImageUrl,
                CategoryItemId = categoryItemId,
                ProductSizes = addProductDto.Sizes.Select(s => new ProductSize { Size = s }).ToList(),
                ProductColors = addProductDto.Colors.Select(c => new Models.ProductColor { Color = c }).ToList(),
                CreatedAt = DateTime.Now
            };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string json = JsonSerializer.Serialize(product, options);


            // Set SizeByLetter or SizeByNumber based on the input sizes
            if (Enum.TryParse<SizeByLetter>(addProductDto.Sizes.FirstOrDefault(), out var sizeByLetter))
            {
                product.SizeByLetter = sizeByLetter;
            }
            else if (int.TryParse(addProductDto.Sizes.FirstOrDefault(), out var sizeByNumber) && sizeByNumber >= 10 && sizeByNumber <= 99)
            {
                product.SizeByNumber = sizeByNumber;
            }

            // Add the product to the repository
            _productRepository.AddProduct(product);
            // Return the created product
            return Ok(product);
        }

        [HttpGet("{productId}")]
        public IActionResult GetProduct(int categoryItemId, int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null || product.CategoryItemId != categoryItemId)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int categoryItemId, int productId, ProductDto updatedProductDto)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null || product.CategoryItemId != categoryItemId)
            {
                return NotFound();
            }

            // Update the product properties
            product.Name = updatedProductDto.Name ?? product.Name;
            product.Brand = updatedProductDto.Brand ?? product.Brand;
            product.Price = updatedProductDto.Price > 0 ? updatedProductDto.Price : product.Price;
            product.Description = updatedProductDto.Description ?? product.Description;
            product.Quantity = updatedProductDto.Quantity > 0 ? updatedProductDto.Quantity : product.Quantity;
            product.Rating = updatedProductDto.Rating > 0 ? updatedProductDto.Rating : product.Rating;
            product.ImageUrl = updatedProductDto.ImageUrl ?? product.ImageUrl;
            product.DiscountRate = updatedProductDto.DiscountRate ?? product.DiscountRate;

            // Update the product in the repository
            _productRepository.UpdateProduct(product);


            // Return the updated product
            return Ok(product);
        }


        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int categoryItemId, int productId)
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
        public IActionResult FilterProducts(int categoryItemId, decimal minPrice, decimal maxPrice, string size, string color)
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
                products = products.Where(p =>
                    (p.SizeByNumber != null && p.SizeByNumber.ToString() == size) ||
                    (p.SizeByLetter != null && p.SizeByLetter.ToString() == size));
            }

            // Apply color filter
            if (!string.IsNullOrEmpty(color) && products.Any(p => p.ProductColors != null))
            {
                products = products.Where(p => p.ProductColors.Any(pc => pc.Color == color));
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
    }
}
