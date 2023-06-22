using System;
using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }
        public string? AdminId { get; set; }

        // Additional properties and methods can be added as per your requirements
    }
}
