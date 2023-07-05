using DShopAPI.Models;

namespace DShopAPI.ViewModels.DTOs
{
        public enum PaymentMethod
        {
            PayPal,
            Card
        }


    public class PaymentDto
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string CardToken { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
