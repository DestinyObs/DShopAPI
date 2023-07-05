using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderSummaryId { get; set; }
        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // Add additional properties as needed, such as product details, SKU, etc.
    }
}
