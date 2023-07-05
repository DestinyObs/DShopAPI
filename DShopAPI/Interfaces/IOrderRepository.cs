using DShopAPI.DTOs;
using DShopAPI.Models;
using System.Collections.Generic;

namespace DShopAPI.Interfaces
{
    public interface IOrderRepository
    {
        Order GetOrderById(int orderId);
        List<Order> GetOrdersByUserId(int userId);
        Order CreateOrder(OrderDto orderDto);
        Order UpdateOrder(int orderId, OrderDto orderDto);
        Order DeleteOrder(int orderId);
    }
}
