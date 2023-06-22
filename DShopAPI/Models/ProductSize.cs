namespace DShopAPI.Models
{

    public class ProductSize
    {
        public int Id { get; set; }
        public string Size { get; set; }

        public int ProductId { get; set; } // Foreign key property
        public Product Product { get; set; } // Navigation property
    }
}
