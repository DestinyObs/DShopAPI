namespace DShopAPI.ViewModels.Dtos
{
    public class CategoryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public UpProdCategoryDto Category { get; set; }
        public List<string> Products { get; set; }
    }
}
