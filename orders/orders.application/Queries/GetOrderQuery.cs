namespace orders.application.Queries
{
    public class GetOrderQuery
    {
        public int OrderId { get; }

        public GetOrderQuery(int orderId)
        {
            OrderId = orderId;
        }
    }
}
