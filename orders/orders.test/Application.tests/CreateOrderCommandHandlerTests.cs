using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using orders.application.CommandHandlers;
using orders.application.Commands;
using orders.application.Dtos;
using orders.domain.Entities;
using orders.domain.Interfaces;
using Xunit;

namespace orders.test.Application.tests
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock = new();
        private readonly Mock<ILogger<CreateOrderCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task Handle_WithValidCommand_ShouldSucceed()
        {
            var handler = new CreateOrderCommandHandler(_repoMock.Object, _loggerMock.Object);
            var command = new CreateOrderCommand(
                1,
                1,
                new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 10 }
                }
            );

            var result = await handler.Handle(command);

            result.IsSuccess.Should().BeTrue();
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidItem_ShouldReturnFailure()
        {
            var handler = new CreateOrderCommandHandler(_repoMock.Object, _loggerMock.Object);
            var command = new CreateOrderCommand(
                1,
                1,
                new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = 0, Quantity = 0, UnitPrice = 0 }
                }
            );

            var result = await handler.Handle(command);

            result.IsSuccess.Should().BeFalse();
        }
    }
}
