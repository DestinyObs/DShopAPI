using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        // Additional properties to handle variable pricing based on location
        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        // You can add more properties here as needed for location-specific pricing or other details
    }
}
