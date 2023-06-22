using System.Collections.Generic;

namespace DShopAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<CategoryItem>? CategoryItems { get; set; }
    }
}
