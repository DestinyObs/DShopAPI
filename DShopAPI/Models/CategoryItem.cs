using System.Collections.Generic;

namespace DShopAPI.Models
{
    public class CategoryItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int CategoryId { get; set; }  // Foreign key property
        public Category Category { get; set; } // Navigation property

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
