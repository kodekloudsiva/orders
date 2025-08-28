using Microsoft.Extensions.Logging;
using orders.application.Commands;
using orders.application.Dtos;
using orders.application.Mappings;
using orders.domain.Common;
using orders.domain.Entities;
using orders.domain.Enums;
using orders.domain.Interfaces;
using orders.shared.Common;
using orders.shared.Results;

namespace orders.application.CommandHandlers
{
    public class UpdateOrderStatusCommandHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderStatusCommandHandler> _logger;

        public UpdateOrderStatusCommandHandler(
            IOrderRepository orderRepository,
            ILogger<UpdateOrderStatusCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand command)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithItemsAsync(command.OrderId);

                if (order == null)
                {
                    return Result<OrderDto>.Failure(new Error(ErrorCodeConstants.VALIDATION_FAILURE, "Order not found"));
                }
                DomainResult<Order>? operationResult = null;

                switch (command.Status)
                {
                    case OrderStatus.Processing:
                        operationResult = order.StartProcessing();
                        break;

                    case OrderStatus.Completed:
                        operationResult = order.Complete();
                        break;

                    case OrderStatus.Cancelled:
                        operationResult = order.Cancel();
                        break;

                    default:
                        var error = new DomainError(
                            ErrorCodeConstants.VALIDATION_FAILURE,                            
                            $"Invalid status transition to {command.Status}",
                            "invalid.status");
                        operationResult = DomainResult<Order>.Failure(error);
                        break;
                }

                if (operationResult.IsFailure)
                {
                    var errors = operationResult.Errors.Select(e => new Error(e.Code, e.Message, e.Field)).ToList();
                    return Result<OrderDto>.Failure(errors);
                }

                await _orderRepository.UpdateAsync(order);
                await _orderRepository.SaveChangesAsync();

                return Result<OrderDto>.Success(order.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status");
                return Result<OrderDto>.Failure(new Error(ErrorCodeConstants.SERVER_ERROR, "Failed to update order status"));
            }
        }
    }
}
