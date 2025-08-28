using orders.domain.Entities;
using orders.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersByUserAsync(int userId);
        Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task SaveChangesAsync();
        Task<IReadOnlyList<Order>> GetOrdersByUserAndStatusAsync(int userId, OrderStatus status);
    }
}
