using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.ViewModels.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Commenting out [Authorize] for now, as you mentioned you don't need it at the moment
    public class CartController : ControllerBase
    {
        private readonly ICartItemRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartController(ICartItemRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }


        // GET: api/cart/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            //var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //if (authenticatedUserId != userId)
            //    return Forbid();

            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
                return NotFound("Cart is empty");

            var cartItems = new List<CartItemResponse>();

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);

                var cartItemResponse = new CartItemResponse
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    Rating = product.Rating,
                    Brand = product.Brand,
                    Quantity = cartItem.Quantity,
                    Price = product.Price,
                    TotalPrice = cartItem.Quantity * product.Price
                };

                cartItems.Add(cartItemResponse);
            }

            return Ok(cartItems);
        }


        [HttpPost("{userId}/add")]
        // [Authorize] // If you decide to add authorization later, you can uncomment [Authorize] here or on specific actions
        public async Task<IActionResult> AddToCart(int userId, [FromBody] CartItemRequest itemRequest)
        {
            //var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //if (authenticatedUserId != userId)
            //    return Forbid();

            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = itemRequest.ProductId,
                Quantity = itemRequest.Quantity
            };

            var product = _productRepository.GetProductById(cartItem.ProductId);
            if (product == null)
                return NotFound("Product not found");

            await _cartRepository.AddToCart(cartItem);
            return Ok("Item added to cart");
        }

        [HttpDelete("{userId}/remove/{itemId}")]
        // [Authorize] // If you decide to add authorization later, you can uncomment [Authorize] here or on specific actions
        public async Task<IActionResult> RemoveFromCart(int userId, int itemId)
        {
            //var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            //if (authenticatedUserId != userId)
            //    return Forbid();

            var cartItem = await _cartRepository.GetCartItemById(itemId);
            if (cartItem == null)
                return NotFound("Cart item not found");

            if (cartItem.UserId != userId)
                return Forbid();

            await _cartRepository.RemoveFromCart(cartItem);
            return Ok("Item removed from cart");
        }


    }

    // ...
}
