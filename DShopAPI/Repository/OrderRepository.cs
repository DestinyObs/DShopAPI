using DShopAPI.DTOs;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DShopAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;

        public OrderRepository()
        {
            _orders = new List<Order>();
        }

        public Order GetOrderById(int orderId)
        {
            return _orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return _orders.Where(o => o.CustomerId == userId).ToList();
        }

        public Order CreateOrder(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderId = GenerateOrderId(), // Provide the implementation for GenerateOrderId() method
                CustomerId = orderDto.UserId,
                CustomerEmail = orderDto.CustomerEmail,
                OrderItems = orderDto.OrderItems,
                TotalAmount = orderDto.TotalAmount,
                OrderDate = DateTime.UtcNow, // Set the order date to the current UTC date and time
                OrderStatus = OrderStatus.Pending, // Set the initial order status
                EstimatedDeliveryDate = CalculateEstimatedDeliveryDate() // Provide the implementation for CalculateEstimatedDeliveryDate() method
            };

            _orders.Add(order);

            return order;
        }


        public Order UpdateOrder(int orderId, OrderDto orderDto)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (existingOrder != null)
            {
                existingOrder.CustomerEmail = orderDto.CustomerEmail;
                existingOrder.OrderItems = orderDto.OrderItems;
                existingOrder.TotalAmount = orderDto.TotalAmount;
                existingOrder.EstimatedDeliveryDate = CalculateEstimatedDeliveryDate();
            }

            return existingOrder;
        }

        public Order DeleteOrder(int orderId)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (existingOrder != null)
            {
                _orders.Remove(existingOrder);
            }

            return existingOrder;
        }

        private int GenerateOrderId()
        {
            // Generate a unique order ID based on your logic
            // This is just a placeholder implementation
            return new Random().Next(1000, 9999);
        }

        private DateTime CalculateEstimatedDeliveryDate()
        {
            // Calculate the estimated delivery date based on your logic
            // This is just a placeholder implementation
            return DateTime.Now.AddDays(7);
        }
    }
}
