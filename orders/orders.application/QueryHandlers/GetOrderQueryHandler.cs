using Microsoft.Extensions.Logging;
using orders.application.Dtos;
using orders.application.Mappings;
using orders.application.Queries;
using orders.domain.Interfaces;
using orders.shared.Common;
using orders.shared.Results;

namespace orders.application.QueryHandlers
{
    public class GetOrderQueryHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<GetOrderQueryHandler> _logger;

        public GetOrderQueryHandler(
            IOrderRepository orderRepository,
            ILogger<GetOrderQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderQuery query)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithItemsAsync(query.OrderId);

                if (order == null)
                {
                    return Result<OrderDto>.Failure(new Error(ErrorCodeConstants.NOT_FOUND, "Order not found"));
                }

                return Result<OrderDto>.Success(order.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order");
                return Result<OrderDto>.Failure(new Error(ErrorCodeConstants.SERVER_ERROR, "Failed to get order"));
            }
        }
    }
}
