using Microsoft.AspNetCore.Mvc;
using DShopAPI.Interfaces;
using DShopAPI.Models;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public SearchController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // No search query provided, return empty result
                return Ok(new List<Product>());
            }

            // Query the product repository to find matching products
            var matchedProducts = _productRepository.SearchProducts(query);

            // Return the matched products
            return Ok(matchedProducts);
        }
    }
}

