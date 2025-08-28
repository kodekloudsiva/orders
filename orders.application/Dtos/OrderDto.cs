namespace orders.application.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; init; }
        public int UserId { get; init; }
        public string Status { get; init; }
        public decimal TotalPrice { get; init; }
        public DateTime CreatedOn { get; init; }
        public List<OrderItemDto> Items { get; init; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalPrice { get; init; }
    }
}
