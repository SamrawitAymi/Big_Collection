using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> DeleteOrderByOrderIdAsync(Guid orderId);
        Task<Order> GetOrderByOrderIdAsync(Guid orderId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<bool> UpdateOrderStatusAsync(int statusId, Guid orderId);
    }
}
