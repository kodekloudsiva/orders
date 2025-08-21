using orders.domain.Common;
using orders.domain.Enums;
using orders.domain.Exceptions;
using orders.shared.Common;

namespace orders.domain.Entities
{
    public class OrderItem: BaseEntity
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        private OrderItem() { } // For EF Core

        public static DomainResult<OrderItem> CreateOrderItem(int productId, int quantity, decimal unitPrice)
        {

            var errors = new List<DomainError>();

            if (productId <= 0)
                errors.Add(new DomainError(
                    ErrorCodeConstants.VALIDATION_FAILURE,
                    "Product ID must be positive",
                    $"orderitem.productId - for {productId}"));
            if (quantity <= 0)
                errors.Add(new DomainError(
                   ErrorCodeConstants.VALIDATION_FAILURE,
                    "Quantity must be positive",
                    $"orderitem.quantity - for {productId}"));
            if (unitPrice <= 0)
                errors.Add(new DomainError(
                    ErrorCodeConstants.VALIDATION_FAILURE,
                    "Unit price must be positive",
                    $"orderitem.unitPrice - for {productId}"));

            if (errors.Count != 0)
                return DomainResult<OrderItem>.Failure(errors);

            var orderItem = new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };

            return DomainResult<OrderItem>.Success(orderItem);
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0) throw new DomainException("Quantity must be positive");
            Quantity = newQuantity;
        }
    }
}
