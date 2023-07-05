using DShopAPI.ViewModels.DTOs;
using System;
using System.Collections.Generic;

namespace DShopAPI.DTOs
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public PaymentDto PaymentDto { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
