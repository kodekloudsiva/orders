using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using orders.application.CommandHandlers;
using orders.application.Commands;
using orders.domain.Entities;
using orders.domain.Enums;
using orders.domain.Interfaces;
using orders.shared.Common;
using Xunit;

namespace orders.test.Application.tests
{
    public class UpdateOrderStatusCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock = new();
        private readonly Mock<ILogger<UpdateOrderStatusCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task Handle_ValidTransition_ShouldUpdateStatus()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;
            var order = Order.Create(1, 1, new[] { item }).Value;

            _repoMock.Setup(r => r.GetOrderWithItemsAsync(1)).ReturnsAsync(order);

            var handler = new UpdateOrderStatusCommandHandler(_repoMock.Object, _loggerMock.Object);

            var command = new UpdateOrderStatusCommand(
                1,
                OrderStatus.Processing
            );

            var result = await handler.Handle(command);

            result.IsSuccess.Should().BeTrue();
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidTransition_ShouldFail()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;
            var order = Order.Create(1, 1, new List<OrderItem> { item }).Value;

            _repoMock.Setup(r => r.GetOrderWithItemsAsync(1)).ReturnsAsync(order);

            var handler = new UpdateOrderStatusCommandHandler(_repoMock.Object, _loggerMock.Object);

            var command = new UpdateOrderStatusCommand(
                1,
                OrderStatus.Completed // Invalid from Pending
            );

            var result = await handler.Handle(command);

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_InvalidOrderStatusToUpdate_ShouldFail()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 100).Value;
            var order = Order.Create(1, 1, new List<OrderItem> { item }).Value;
            _repoMock.Setup(x => x.GetOrderWithItemsAsync(1)).ReturnsAsync(order);
            var command = new UpdateOrderStatusCommand(
                1,
                OrderStatus.Pending
            );
            var handler = new UpdateOrderStatusCommandHandler(_repoMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command);
            result.IsSuccess.Should().BeFalse();
            result?.Errors?.Any().Should().BeTrue();
        }

        [Fact]
        public async Task Handle_InvalidOrderInSystem_ShouldFail()
        {            
            var command = new UpdateOrderStatusCommand(
                1,
                OrderStatus.Processing
            );

            Order? order = null;
            _repoMock.Setup(x => x.GetOrderWithItemsAsync(1067)).ReturnsAsync(order);

            var handler = new UpdateOrderStatusCommandHandler(_repoMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command);
            result.IsSuccess.Should().BeFalse();
            result?.Errors?.Any().Should().BeTrue();
            result?.Error.Code.Equals(ErrorCodeConstants.VALIDATION_FAILURE);
        }

        [Fact]
        public async Task Handle_CancelTransition_ShouldSucceed()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;
            var order = Order.Create(1, 1, new List<OrderItem> { item }).Value;

            _repoMock.Setup(r => r.GetOrderWithItemsAsync(1)).ReturnsAsync(order);

            var handler = new UpdateOrderStatusCommandHandler(_repoMock.Object, _loggerMock.Object);

            var command = new UpdateOrderStatusCommand(
                1,
                OrderStatus.Cancelled
            );

            var result = await handler.Handle(command);

            result.IsSuccess.Should().BeTrue();
            result.Value.Status.Equals(OrderStatus.Cancelled);
        }
    }
}
