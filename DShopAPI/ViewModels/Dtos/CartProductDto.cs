namespace DShopAPI.ViewModels.Dtos
{
    public class CartProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public float Rating { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
