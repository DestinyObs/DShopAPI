using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class CustomerOrderInformation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Shipping Address is required.")]
        public string ShippingAddress { get; set; }

        public string? StreetAddress { get; set; }

        [Required(ErrorMessage = "Town/City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country/State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postcode/Zip is required.")]
        public string PostcodeZip { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Account Password is required.")]
        [DataType(DataType.Password)]
        public string AccountPassword { get; set; }

        public string? OrderNotes { get; set; }
    }
}
