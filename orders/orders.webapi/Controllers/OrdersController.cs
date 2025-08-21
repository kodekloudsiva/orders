using Microsoft.AspNetCore.Mvc;
using orders.application.CommandHandlers;
using orders.application.Commands;
using orders.application.Queries;
using orders.application.QueryHandlers;
using orders.domain.Enums;
using orders.shared.Common;
using orders.shared.Results;

namespace orders.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderCommandHandler _createOrderHandler;
        private readonly GetOrderQueryHandler _getOrderHandler;
        private readonly UpdateOrderStatusCommandHandler _updateStatusHandler;
        private readonly GetUserOrdersQueryHandler _getUserOrdersHandler;

        public OrdersController(
            CreateOrderCommandHandler createOrderHandler,
            GetOrderQueryHandler getOrderHandler,
            UpdateOrderStatusCommandHandler updateStatusHandler,
            GetUserOrdersQueryHandler getUserOrdersHandler)
        {
            _createOrderHandler = createOrderHandler;
            _getOrderHandler = getOrderHandler;
            _updateStatusHandler = updateStatusHandler;
            _getUserOrdersHandler = getUserOrdersHandler;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var result = await _getOrderHandler.Handle(new GetOrderQuery(orderId));
            return HandleResult(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId, [FromQuery] OrderStatus? status)
        {
            var result = await _getUserOrdersHandler.Handle(new GetUserOrdersQuery(userId, status));
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _createOrderHandler.Handle(command);
            return HandleResult(result, nameof(GetOrder), result.IsSuccess ? new { orderId = result.Value.OrderId } : null);
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus status)
        {
            var result = await _updateStatusHandler.Handle(new UpdateOrderStatusCommand(orderId, status));
            return HandleResult(result);
        }

        private IActionResult HandleResult<T>(Result<T> result, string? actionName = null, object? routeValues = null)
        {
            if (result.IsSuccess)
            {
                return actionName != null
                    ? CreatedAtAction(actionName, routeValues, result.Value)
                    : Ok(result.Value);
            }

            switch (result?.Error?.Code)
            {
                case ErrorCodeConstants.NOT_FOUND:
                    return result?.Errors?.Count > 0 ? NotFound(result?.Errors) : NotFound(result?.Error);

                case ErrorCodeConstants.VALIDATION_FAILURE:
                    return result?.Errors?.Count > 0 ? BadRequest(result?.Errors) : BadRequest(result?.Error);

                default:
                    return StatusCode(500, result?.Error);
            }
        }
    }
}
