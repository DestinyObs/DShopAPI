using System.ComponentModel.DataAnnotations;

namespace DShopAPI.ViewModels.Dtos
{
    public class CartItemResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        [Range(0, 999999.99, ErrorMessage = "Price must be between 0 and 999999.99")]
        public decimal Price { get; set; }

        [Range(0, 999999.99, ErrorMessage = "TotalPrice must be between 0 and 999999.99")]
        public decimal TotalPrice { get; set; }
    }
}
