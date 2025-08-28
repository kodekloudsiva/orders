using orders.domain.Enums;

namespace orders.application.Commands
{
    public class UpdateOrderStatusCommand
    {
        public int OrderId { get; }
        public OrderStatus Status { get; }

        public UpdateOrderStatusCommand(int orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }
    }

}
