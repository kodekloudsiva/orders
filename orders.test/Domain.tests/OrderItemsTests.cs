using FluentAssertions;
using orders.domain.Entities;
using orders.domain.Exceptions;
using Xunit;

namespace orders.test.Domain.tests
{
    public class OrderItemTests
    {
        [Fact]
        public void CreateOrderItem_WithValidData_ShouldSucceed()
        {
            var result = OrderItem.CreateOrderItem(1, 2, 10);

            result.IsSuccess.Should().BeTrue();
            result.Value.ProductId.Should().Be(1);
            result.Value.Quantity.Should().Be(2);
            result.Value.TotalPrice.Should().Be(20);
        }

        [Fact]
        public void CreateOrderItem_WithInvalidData_ShouldReturnErrors()
        {
            var result = OrderItem.CreateOrderItem(0, 0, 0);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public void UpdateQuantity_WithValidQuantity_ShouldUpdate()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;

            item.UpdateQuantity(5);

            item.Quantity.Should().Be(5);
        }

        [Fact]
        public void UpdateQuantity_WithInvalidQuantity_ShouldThrow()
        {
            var item = OrderItem.CreateOrderItem(1, 1, 10).Value;

            Action act = () => item.UpdateQuantity(0);

            act.Should().Throw<DomainException>().WithMessage("Quantity must be positive");
        }
    }
}
