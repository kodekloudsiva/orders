using orders.domain.Enums;

namespace orders.application.Queries
{
    public class GetUserOrdersQuery
    {
        public int UserId { get; }
        public OrderStatus? Status { get; }

        public GetUserOrdersQuery(int userId, OrderStatus? status = null)
        {
            UserId = userId;
            Status = status;
        }
    }
}
