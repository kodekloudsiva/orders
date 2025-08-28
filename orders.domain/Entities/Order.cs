using orders.domain.Common;
using orders.domain.Enums;
using orders.shared.Common;

namespace orders.domain.Entities;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = new List<OrderItem>();

    public int OrderId { get; private init; }
    public int UserId { get; private init; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice => _items.Sum(i => i.TotalPrice);
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // Private constructor for EF Core
    private Order() { }

    // Static factory method with validation
    public static DomainResult<Order> Create(
        int orderId,
        int userId,
        IEnumerable<OrderItem> items)
    {
        var errors = new List<DomainError>();

        if (userId <= 0)
            errors.Add(new DomainError(
                ErrorCodeConstants.VALIDATION_FAILURE,
                "User ID must be positive",
                "order.invalid_user_id"));

        if (items == null || !items.Any())
            errors.Add(new DomainError(
                ErrorCodeConstants.VALIDATION_FAILURE,
                "Order must contain items",
                "order.empty_items"));

        if (errors.Count > 0)
            return DomainResult<Order>.Failure(errors);

        var order = new Order
        {
            OrderId = orderId,
            UserId = userId,
            Status = OrderStatus.Pending,
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = DateTime.UtcNow
        };

        order._items.AddRange(items);
        return DomainResult<Order>.Success(order);
    }

    // Domain operations with result pattern
    public DomainResult<Order> AddItem(OrderItem item)
    {
        if (item == null)
            return DomainResult<Order>.Failure(
                new DomainError(
                    ErrorCodeConstants.VALIDATION_FAILURE,
                    "Item cannot be null",
                    "order.null_item"));

        _items.Add(item);
        UpdateLastModified();
        return DomainResult<Order>.Success(this);
    }

    public DomainResult<Order> StartProcessing()
    {
        if (Status != OrderStatus.Pending)
            return DomainResult<Order>.Failure(
                new DomainError(
                    ErrorCodeConstants.VALIDATION_FAILURE,
                    $"Cannot start processing from {Status} status",
                    "order.invalid_status_transition"));

        Status = OrderStatus.Processing;
        UpdateLastModified();
        return DomainResult<Order>.Success(this);
    }

    public DomainResult<Order> Complete()
    {
        if (Status != OrderStatus.Processing)
            return DomainResult<Order>.Failure(
                new DomainError(
                    ErrorCodeConstants.VALIDATION_FAILURE,
                    $"Cannot complete from {Status} status",
                    "order.invalid_status_transition"));

        Status = OrderStatus.Completed;
        UpdateLastModified();
        return DomainResult<Order>.Success(this);
    }

    public DomainResult<Order> Cancel()
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            return DomainResult<Order>.Failure(
                new DomainError(
                    ErrorCodeConstants.VALIDATION_FAILURE,                    
                    $"Cannot cancel from {Status} status",
                    "order.invalid_status_transition"));

        Status = OrderStatus.Cancelled;
        UpdateLastModified();
        return DomainResult<Order>.Success(this);
    }

    private void UpdateLastModified() => LastModifiedOn = DateTime.UtcNow;
}