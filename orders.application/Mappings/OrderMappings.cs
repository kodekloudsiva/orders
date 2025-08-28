using orders.application.Dtos;
using orders.domain.Entities;

namespace orders.application.Mappings
{
    public static class OrderMappings
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                CreatedOn = order.CreatedOn,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
    }
}
