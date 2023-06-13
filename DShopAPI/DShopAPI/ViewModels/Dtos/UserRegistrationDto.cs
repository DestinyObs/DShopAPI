using System.ComponentModel.DataAnnotations;

namespace DShopAPI.ViewModels.Dtos
{
    public class UserRegistrationDto
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        public string? ConfirmationCode { get; set; }
    }
}
