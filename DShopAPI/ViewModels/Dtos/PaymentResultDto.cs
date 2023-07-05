namespace DShopAPI.ViewModels.DTOs
{
    public class PaymentResultDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string? TransactionId { get; set; }
        public string PaymentId { get; set; }

    }
}
