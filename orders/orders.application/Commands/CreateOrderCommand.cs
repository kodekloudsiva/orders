using orders.application.Dtos;

namespace orders.application.Commands
{
    public class CreateOrderCommand
    {
        public int OrderId { get; }
        public int UserId { get; }
        public List<OrderItemDto> Items { get; }

        public CreateOrderCommand(int orderId, int userId, List<OrderItemDto> items)
        {
            OrderId = orderId;
            UserId = userId;
            Items = items;
        }
    }
}
