//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using DShopAPI.Interfaces;
//using DShopAPI.Models;
//using DShopAPI.ViewModels.Dtos;
//using DShopAPI.Migrations;

//namespace DShopAPI.Controllers
//{
//    [Authorize] // Requires authentication for all endpoints in this controller
//    [ApiController]
//    [Route("api/wishlist")]
//    public class WishlistController : ControllerBase
//    {
//        private readonly IWishlistRepository _wishlistRepository;
//        private readonly IUserRepository _userRepository;

//        public WishlistController(IWishlistRepository wishlistRepository, IUserRepository userRepository)
//        {
//            _wishlistRepository = wishlistRepository;
//            _userRepository = userRepository;
//        }

//        [HttpPost]
//        [Route("add")]
//        public IActionResult AddToWishlist(int productId)
//        {
//            // Get the authenticated user's ID
//            string userId = GetUserId();

//            // Check if the user exists
//            User user = _userRepository.GetUserById(userId);
//            if (user == null)
//            {
//                return NotFound("User not found");
//            }

//            // Check if the product exists
//            Product product = _wishlistRepository.GetProduct(productId);
//            if (product == null)
//            {
//                return NotFound("Product not found");
//            }

//            // Add the product to the user's wishlist
//            _wishlistRepository.AddToWishlist(userId, product);

//            return Ok("Product added to wishlist");
//        }

//        [HttpGet]
//        [Route("get")]
//        public IActionResult GetWishlist()
//        {
//            // Get the authenticated user's ID
//            string userId = GetUserId();

//            // Get the user's wishlist
//            IEnumerable<Product> wishlist = _wishlistRepository.GetWishlist(userId);

//            // Map the wishlist products to DTOs
//            IEnumerable<ProductDto> wishlistDto = wishlist.Select(p => new ProductDto
//            {
//                Id = p.Id,
//                Name = p.Name,
//                // Include other relevant properties
//            });

//            return Ok(wishlistDto);
//        }

//        [HttpDelete]
//        [Route("remove")]
//        public IActionResult RemoveFromWishlist(int productId)
//        {
//            // Get the authenticated user's ID
//            string userId = GetUserId();

//            // Check if the product exists in the user's wishlist
//            Product product = _wishlistRepository.GetProduct(userId, productId);
//            if (product == null)
//            {
//                return NotFound("Product not found in wishlist");
//            }

//            // Remove the product from the user's wishlist
//            _wishlistRepository.RemoveFromWishlist(userId, productId);

//            return Ok("Product removed from wishlist");
//        }

//        // Helper method to get the authenticated user's ID
//        private string GetUserId()
//        {
//            // Retrieve the user ID from the authentication token or session
//            // Implement your own logic to extract the user ID based on your authentication method
//            // For example, using JWT authentication: User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            return "user123"; // Replace with your actual implementation
//        }
//    }
//}
