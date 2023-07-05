using DShopAPI.DTOs;
using DShopAPI.Interfaces;
using DShopAPI.ViewModels.DTOs;
using Paystack.Net;
using Paystack.Net.Models.Transactions;
using Paystack.Net.SDK.Models.Charge;
using PayStack.Net;
using System.Net;
using System.Threading.Tasks;

namespace DShopAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaystackApi _paystackApi;

        public PaymentService(string paystackSecretKey)
        {
            _paystackApi = new PaystackApi(paystackSecretKey);
        }

        public async Task<PaymentResultDto> ProcessPayment(PaymentDto paymentDto)
        {
            if (paymentDto.PaymentMethod == PaymentMethod.PayPal)
            {
                return await ProcessPaymentWithPayPal(paymentDto);
            }
            else if (paymentDto.PaymentMethod == PaymentMethod.Card)
            {
                return await ProcessPaymentWithCard(paymentDto);
            }
            else
            {
                // Handle other payment methods if applicable
                return null;
            }
        }

        public async Task<PaymentResultDto> ProcessPaymentWithPayPal(PaymentDto paymentDto)
        {
            // Implement PayPal payment processing logic if applicable
        }

        public async Task<PaymentResultDto> ProcessPaymentWithCard(PaymentDto paymentDto)
        {
            var chargeOptions = new ChargeRequest
            {
                Amount = (int)(paymentDto.Amount * 100), // Amount in kobo (Naira's smallest currency unit)
                Email = paymentDto.Email,
                Card = new ChargeCard
                {
                    Number = paymentDto.CardNumber,
                    Cvv = paymentDto.CVV,
                    ExpiryMonth = paymentDto.ExpiryMonth,
                    ExpiryYear = paymentDto.ExpiryYear
                },
                Metadata = new { order_id = paymentDto.OrderId }
            };

            var chargeResponse = await _paystackApi.Transactions.ChargeCard(chargeOptions);

            if (chargeResponse.Status == true && chargeResponse.Data.Status == "success")
            {
                var paymentResult = new PaymentResultDto
                {
                    IsSuccessful = true,
                    Message = "Payment processed successfully with Card in Naira.",
                    TransactionId = chargeResponse.Data.Reference,
                    PaymentId = chargeResponse.Data.Id
                };

                return paymentResult;
            }
            else
            {
                var paymentResult = new PaymentResultDto
                {
                    IsSuccessful = false,
                    Message = "Payment failed with Card in Naira."
                };

                return paymentResult;
            }
        }
    }
}
