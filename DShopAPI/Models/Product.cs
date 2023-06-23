using System;
using System.Collections.Generic;
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

        [Range(10, 99)]
        public int SizeByNumber { get; set; }

        public SizeByLetter SizeByLetter { get; set; }

        public int CategoryItemId { get; set; }
        public CategoryItem CategoryItem { get; set; }

        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<Color> Colors { get; set; }
    }

    public enum SizeByLetter
    {
        S,
        M,
        L,
        XL
    }

    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    
}
