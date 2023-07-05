using DShopAPI.DTOs;
using DShopAPI.Interfaces;
using DShopAPI.ViewModels.DTOs;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using Stripe;
using System.Net;
using System.Threading.Tasks;

namespace DShopAPI.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentResultDto> ProcessPayment(PaymentDto paymentDto)
        {
            if (paymentDto.PaymentMethod == DShopAPI.ViewModels.DTOs.PaymentMethod.PayPal)
            {
                return await ProcessPaymentWithPayPal(paymentDto);
            }
            else if (paymentDto.PaymentMethod == DShopAPI.ViewModels.DTOs.PaymentMethod.Card)
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
            // Set your PayPal environment credentials
            var environment = new SandboxEnvironment("YOUR_PAYPAL_CLIENT_ID", "YOUR_PAYPAL_CLIENT_SECRET");
            var client = new PayPalHttpClient(environment);

            // Create an order request
            var orderRequest = new OrdersCreateRequest(); 
            orderRequest.Prefer("return=representation");
            orderRequest.RequestBody(new OrderRequest
            {
                /* Set the properties of the OrderRequest using paymentDto */
            });



            // Create the order and get the response
            var response = await client.Execute(orderRequest);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var order = response.Result<PayPalCheckoutSdk.Orders.Order>();

                var paymentResult = new PaymentResultDto
                {
                    IsSuccessful = true,
                    Message = "Payment processed successfully with PayPal.",
                    PaymentId = order.Id,
                    TransactionId = order.Id
                };

                return paymentResult;
            }
            else
            {
                var paymentResult = new PaymentResultDto
                {
                    IsSuccessful = false,
                    Message = "Payment failed with PayPal."
                };

                return paymentResult;
            }
        }

        public async Task<PaymentResultDto> ProcessPaymentWithCard(PaymentDto paymentDto)
        {
            // Set your Stripe API key
            StripeConfiguration.ApiKey = "YOUR_STRIPE_API_KEY";

            // Create a Stripe charge
            var options = new ChargeCreateOptions
            {
                Amount = (long)(paymentDto.Amount * 100), // Amount in cents
                Currency = "usd",
                Description = "Payment for Order",
                Source = paymentDto.CardToken // Token representing the card details
            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            if (charge.Status == "succeeded")
            {
                var paymentResult = new PaymentResultDto
                {
                    IsSuccessful = true,
                    Message = "Payment processed successfully with Card."
                };

                return paymentResult;
            }
            else
            {
                var paymentResult = new PaymentResultDto
                {
                    IsSuccessful = false,
                    Message = "Payment failed with Card."
                };

                return paymentResult;
            }
        }

    }
}
