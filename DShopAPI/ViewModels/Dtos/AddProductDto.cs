using System.ComponentModel.DataAnnotations;

public class AddProductDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public string Description { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public string Brand { get; set; }

    public string ImageUrl { get; set; }

    public List<string> Sizes { get; set; }

    public List<string> Colors { get; set; }
}
