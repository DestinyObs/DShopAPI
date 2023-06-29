namespace DShopAPI.ViewModels.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public int Rating { get; set; }
        public string ImageUrl { get; set; }
        public string DiscountRate { get; set; }
        public int SizeByNumber { get; set; }
        public int SizeByLetter { get; set; }
        public int CategoryItemId { get; set; }
        public CategoryItemDto CategoryItem { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; }
        public List<ProductColorDto> ProductColors { get; set; }
    }
}
