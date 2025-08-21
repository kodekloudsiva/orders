using FluentAssertions;
using orders.domain.Entities;
using orders.domain.Enums;
using Xunit;

namespace orders.test.Domain.tests
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrder_WithValidData_ShouldSucceed()
        {
            var item = OrderItem.CreateOrderItem(1, 2, 10).Value;
            var result = Order.Create(1, 1, new[] { item });

            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPrice.Should().Be(20);
        }

        [Fact]
        public void CreateOrder_WithEmptyItems_ShouldFail()
        {
            var result = Order.Create(1, 1, new List<OrderItem>());

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void StartProcessing_WithPendingStatus_ShouldSucceed()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;
            var order = Order.Create(1, 1, new[] { item }).Value;

            var result = order.StartProcessing();

            result.IsSuccess.Should().BeTrue();
            order.Status.Should().Be(OrderStatus.Processing);
        }

        [Fact]
        public void Complete_WithInvalidState_ShouldFail()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;
            var order = Order.Create(1, 1, new[] { item }).Value;

            var result = order.Complete();

            result.IsFailure.Should().BeTrue();
        }
    }
}
