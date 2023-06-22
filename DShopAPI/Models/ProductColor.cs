namespace DShopAPI.Models
{
    public class ProductColor
    {
        public int Id { get; set; }
        public string Color { get; set; }

        public int ProductId { get; set; } // Foreign key property
        public Product Product { get; set; } // Navigation property
    }

}
