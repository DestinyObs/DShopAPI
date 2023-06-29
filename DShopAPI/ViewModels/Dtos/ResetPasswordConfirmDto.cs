using System.ComponentModel.DataAnnotations;

namespace DShopAPI.ViewModels.Dtos
{
    public class ResetPasswordConfirmDto
    {
        [Required]
        public string ResetToken { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
