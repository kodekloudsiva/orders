using Microsoft.Extensions.Logging;
using orders.application.Dtos;
using orders.application.Mappings;
using orders.application.Queries;
using orders.domain.Entities;
using orders.domain.Interfaces;
using orders.shared.Common;
using orders.shared.Results;

namespace orders.application.QueryHandlers
{
    public class GetUserOrdersQueryHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetUserOrdersQueryHandler> _logger;

        public GetUserOrdersQueryHandler(
            IOrderRepository orderRepository,
            ILogger<GetUserOrdersQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> Handle(GetUserOrdersQuery query)
        {
            try
            {
                IReadOnlyList<Order> orders;

                if (query.Status.HasValue)
                {
                    orders = await _orderRepository.GetOrdersByUserAndStatusAsync(query.UserId, query.Status.Value);
                }
                else
                {
                    orders = await _orderRepository.GetOrdersByUserAsync(query.UserId);
                }

                if (orders == null || !orders.Any())
                {
                    return Result<IReadOnlyList<OrderDto>>.Failure(
                        new Error(ErrorCodeConstants.NOT_FOUND, "No orders found for this user"));
                }

                var orderDtos = orders.Select(o => o.ToDto()).ToList();
                return Result<IReadOnlyList<OrderDto>>.Success(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user orders");
                return Result<IReadOnlyList<OrderDto>>.Failure(
                    new Error(ErrorCodeConstants.SERVER_ERROR, "Failed to retrieve user orders"));
            }
        }
    }
}
