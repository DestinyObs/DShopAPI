using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.ViewModels.Dtos;
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
        public IActionResult CreateProduct(int categoryId, int categoryItemId, AddProductDto productDto)
        {
            // Map the DTO to the Product entity
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                Quantity = productDto.Quantity,
                Brand = productDto.Brand,
                ImageUrl = productDto.ImageUrl,
                DiscountRate = productDto.DiscountRate,
                CategoryItemId = categoryItemId,
                Colors = productDto.Colors.Select(c => new Models.Color { Name = c }).ToList()
            };

            // Set SizeByLetter or SizeByNumber based on the input sizes
            if (Enum.TryParse<SizeByLetter>(productDto.Sizes.FirstOrDefault(), out var sizeByLetter))
            {
                product.SizeByLetter = sizeByLetter;
            }
            else if (int.TryParse(productDto.Sizes.FirstOrDefault(), out var sizeByNumber) && sizeByNumber >= 10 && sizeByNumber <= 99)
            {
                product.SizeByNumber = sizeByNumber;
            }

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
                products = products.Where(p =>
                    (p.SizeByNumber != null && p.SizeByNumber.ToString() == size) ||
                    (p.SizeByLetter != null && p.SizeByLetter.ToString() == size));
            }

            // Apply color filter
            if (!string.IsNullOrEmpty(color) && products.Any(p => p.Colors != null))
            {
                products = products.Where(p => p.Colors.Any(c => c.Name == color));
            }


            return Ok(products);
            //// Filter the properties to only include the required fields
            //var filteredProducts = products.Select(p => new
            //{
            //    p.Name,
            //    p.Brand,
            //    p.Price,
            //    p.DiscountRate,
            //    p.ImageUrl,
            //    p.Rating,
            //    p.Colors

            //});

            //return Ok(filteredProducts);
        }


    }
}

