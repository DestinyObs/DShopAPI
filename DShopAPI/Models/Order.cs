using DShopAPI.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order Status is required.")]
        public OrderStatus OrderStatus { get; set; }

        public int CustomerOrderInformationId { get; set; }
        public CustomerOrderInformation CustomerOrderInformation { get; set; }

        public string OrderNotes { get; set; }

        // Additional properties as needed
        public decimal TotalAmount { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }

        //public int PaymentMethodId { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }

        // Navigation property for OrderItems (collection of order items)
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        // Other methods and properties
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered
    }
}
