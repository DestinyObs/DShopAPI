using System;
using System.ComponentModel.DataAnnotations;

namespace DShopAPI.Models
{
    public class OrderHistory
    {
        public int OrderHistoryId { get; set; }
        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime Date { get; set; }

        // Additional properties as needed
        // ...
    }
}
