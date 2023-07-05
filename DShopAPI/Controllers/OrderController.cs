using DShopAPI.DTOs;
using DShopAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using DShopAPI.ViewModels.DTOs;
using DShopAPI.Models;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public OrderController(IOrderRepository orderRepository, IPaymentService paymentService, IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _paymentService = paymentService;
            _emailService = emailService;
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(int orderId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            var orders = _orderRepository.GetOrdersByUserId(userId);
            return Ok(orders);
        }
        public class PlaceOrderRequest
        {
            public CustomerOrderInformation CustomerOrderInformation { get; set; }
            public OrderSummary OrderSummary { get; set; }
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest placeOrderRequest)
        {
            CustomerOrderInformation customerOrderInformation = placeOrderRequest.CustomerOrderInformation;
            OrderSummary orderSummary = placeOrderRequest.OrderSummary;

            var orderDto = new OrderDto
            {
                CustomerEmail = customerOrderInformation.Email,
                TotalAmount = orderSummary.TotalAmount,
                CustomerName = $"{customerOrderInformation.FirstName} {customerOrderInformation.LastName}",
                OrderItems = orderSummary.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList(),
                // Set other properties as needed
            };

            var createdOrder = _orderRepository.CreateOrder(orderDto);
            if (createdOrder == null)
                return BadRequest("Failed to place order.");

            var paymentResult = await _paymentService.ProcessPayment(orderDto.PaymentDto);
            if (!paymentResult.IsSuccessful)
                return BadRequest("Payment processing failed.");


            // Send order confirmation email
            //var emailBody = ComposeOrderConfirmationEmail();
            //if (!SendEmail(createdOrder.CustomerEmail, "Order Confirmation", emailBody))
            //    return BadRequest("Failed to send order confirmation email.");

            return Ok(createdOrder);
        }

        [HttpPost("pay/paypal")]
        public async Task<IActionResult> ProcessPaymentWithPayPal(PaymentDto paymentDto)
        {
            var paymentResult = await _paymentService.ProcessPaymentWithPayPal(paymentDto);
            if (!paymentResult.IsSuccessful)
                return BadRequest("Payment processing with PayPal failed.");

            return Ok(paymentResult);
        }

        [HttpPost("pay/card")]
        public async Task<IActionResult> ProcessPaymentWithCard(PaymentDto paymentDto)
        {
            var paymentResult = await _paymentService.ProcessPaymentWithCard(paymentDto);
            if (!paymentResult.IsSuccessful)
                return BadRequest("Payment processing with card failed.");

            return Ok(paymentResult);
        }

        [HttpPut("{orderId}")]
        public IActionResult UpdateOrder(int orderId, OrderDto orderDto)
        {
            var updatedOrder = _orderRepository.UpdateOrder(orderId, orderDto);
            if (updatedOrder == null)
                return NotFound();

            return Ok(updatedOrder);
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            var deletedOrder = _orderRepository.DeleteOrder(orderId);
            if (deletedOrder == null)
                return NotFound();

            return Ok(deletedOrder);
        }

        private string ComposeOrderConfirmationEmail(OrderDto orderDto)
        {
            // Compose the email body with order details
            // You can customize the email template as needed
            string emailBody = $"Dear {orderDto.CustomerName},\n\n";
            emailBody += "Thank you for your order!\n\n";
            emailBody += "Order Details:\n";
            emailBody += $"Order ID: {orderDto.OrderId}\n";
            emailBody += $"Items Purchased: {orderDto.OrderItems.Count}\n";
            emailBody += $"Total Amount: {orderDto.TotalAmount:C}\n";
            emailBody += $"Estimated Delivery Date: {orderDto.EstimatedDeliveryDate:d}\n\n";
            emailBody += "We appreciate your business.\n\n";
            emailBody += "Best regards,\n";
            emailBody += "Your Store";

            return emailBody;
        }

        private bool SendEmail(string email, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("your_smtp_server", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("your_username", "your_password");
                smtpClient.EnableSsl = true;

                mail.From = new MailAddress("your_username");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
        }
    }
}
