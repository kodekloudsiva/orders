using Microsoft.Extensions.Logging;
using orders.application.Commands;
using orders.application.Dtos;
using orders.application.Mappings;
using orders.domain.Entities;
using orders.domain.Interfaces;
using orders.shared.Common;
using orders.shared.Results;

namespace orders.application.CommandHandlers
{
    public class CreateOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result<OrderDto>> Handle(CreateOrderCommand command)
        {
            try
            {
                var orderItems = command.Items
                    .Select(i => OrderItem.CreateOrderItem(i.ProductId, i.Quantity, i.UnitPrice))
                    .ToList();

                // Validate empty order
                if (orderItems.Count == 0)
                {
                    return Result<OrderDto>.Failure(
                        new Error(ErrorCodeConstants.VALIDATION_FAILURE, "Order must contain at least one item", "order.empty_items")
                    );
                }

                // Collect item creation errors
                var errors = orderItems
                    .Where(item => item.IsFailure)
                    .SelectMany((item, index) =>
                        item.Errors.Select(e => new Error(e.Code, e.Message, $"product item {index}")))
                    .ToList();

                if (errors.Count != 0)
                {
                    return Result<OrderDto>.Failure(errors);
                }

                // Create order
                // Select only successful items (defensive coding)
                var validItems = orderItems
                    .Where(item => item.IsSuccess && item.Value != null)
                    .Select(item => item.Value)
                    .ToList();

                if (validItems.Count == 0)
                {
                    return Result<OrderDto>.Failure(
                        new Error(ErrorCodeConstants.VALIDATION_FAILURE, "No valid items to create an order", "order.no_valid_items")
                    );
                }

                // Create order
                var orderResult = Order.Create(command.OrderId, command.UserId, validItems);

                if (orderResult.IsFailure)
                {
                    var orderErrors = orderResult.Errors
                        .Select(e => new Error(e.Code, e.Message, e.Field))
                        .ToList();

                    return Result<OrderDto>.Failure(orderErrors);
                }

                // Persist order
                await _orderRepository.AddAsync(orderResult.Value);
                await _orderRepository.SaveChangesAsync();

                return Result<OrderDto>.Success(orderResult.Value.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return Result<OrderDto>.Failure(new Error(ErrorCodeConstants.SERVER_ERROR, "Failed to create order"));
            }
        }
    }

}
