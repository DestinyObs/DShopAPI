using System.ComponentModel.DataAnnotations;

namespace DShopAPI.ViewModels.Dtos
{
    public class ResetPasswordDto
    {
        public string ResetToken { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordInitiateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    
}
