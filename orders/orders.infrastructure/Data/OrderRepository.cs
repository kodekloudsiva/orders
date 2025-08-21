using Microsoft.EntityFrameworkCore;
using orders.domain.Entities;
using orders.domain.Enums;
using orders.domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.infrastructure.Data
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserAndStatusAsync(int userId, OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Status == status)
                .Include(o => o.Items)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
