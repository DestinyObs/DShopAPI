using System.ComponentModel.DataAnnotations;

namespace DShopAPI.ViewModels.Dtos
{
    public class ResetPasswordInitiateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    
}
