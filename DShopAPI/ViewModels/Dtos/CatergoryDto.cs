namespace DShopAPI.ViewModels.Dtos
{
    public class UpProdCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> CategoryItems { get; set; }
    }
}
