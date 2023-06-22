using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DShopAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public float Rating { get; set; }
        public string ImageUrl { get; set; }
        public string DiscountRate { get; set; }
        public int CategoryItemId { get; set; }  // Foreign key property

        public CategoryItem CategoryItem { get; set; } // Navigation property

        public ICollection<ProductColor> Colors { get; set; } // Collection of available colors
        public ICollection<ProductSize> Sizes { get; set; } // Collection of available sizes
    }
}
