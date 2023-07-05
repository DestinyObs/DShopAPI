using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class OrderSummary
    {
        public int OrderSummaryId { get; set; }
        public int CustomerOrderInformationId { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [Required(ErrorMessage = "Subtotal is required.")]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        //[Required(ErrorMessage = "Payment Method is required.")]
        //public PaymentMethod PaymentMethod { get; set; }
    }
}
