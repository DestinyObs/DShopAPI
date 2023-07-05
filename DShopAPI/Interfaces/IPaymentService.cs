using DShopAPI.ViewModels.DTOs;

namespace DShopAPI.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPayment(PaymentDto paymentDto);
        Task<PaymentResultDto> ProcessPaymentWithCard(PaymentDto paymentDto);
        Task<PaymentResultDto> ProcessPaymentWithPayPal(PaymentDto paymentDto);
    }
}
