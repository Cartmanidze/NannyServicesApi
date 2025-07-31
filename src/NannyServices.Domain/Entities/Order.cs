using NannyServices.Domain.Common;
using NannyServices.Domain.Enums;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime LastEditDate { get; private set; }

    private readonly List<OrderLine> _orderLines = [];
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

    public Money TotalAmount => _orderLines.Aggregate(
        Money.Zero(),
        (total, line) => total + line.TotalPrice
    );

    public bool CanBeModified => Status is OrderStatus.Created or OrderStatus.InProgress;

    /// <summary>
    /// For EF Core
    /// </summary>
    private Order() { }

    public Order(Customer customer)
    {
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        CustomerId = customer.Id;
        Status = OrderStatus.Created;
        LastEditDate = DateTime.UtcNow;
    }

    public void AddOrderLine(Product product, int count)
    {
        if (!CanBeModified)
        {
            throw new InvalidOperationException($"Cannot modify order with status {Status}");
        }

        var existingLine = _orderLines.FirstOrDefault(ol => ol.ProductId == product.Id);
        if (existingLine != null)
        {
            existingLine.UpdateCount(existingLine.Count + count);
        }
        else
        {
            var orderLine = new OrderLine(product, count, product.Price);
            orderLine.SetOrderId(Id);
            _orderLines.Add(orderLine);
        }

        LastEditDate = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void RemoveOrderLine(Guid orderLineId)
    {
        if (!CanBeModified)
        {
            throw new InvalidOperationException($"Cannot modify order with status {Status}");
        }

        var orderLine = _orderLines.FirstOrDefault(ol => ol.Id == orderLineId);
        if (orderLine != null)
        {
            _orderLines.Remove(orderLine);
            LastEditDate = DateTime.UtcNow;
            UpdateTimestamp();
        }
    }

    public void UpdateOrderLineCount(Guid orderLineId, int count)
    {
        if (!CanBeModified)
        {
            throw new InvalidOperationException($"Cannot modify order with status {Status}");
        }
        
        var orderLine = _orderLines.FirstOrDefault(ol => ol.Id == orderLineId);
        if (orderLine == null)
        {
            throw new ArgumentException("Order line not found", nameof(orderLineId));
        }
        
        orderLine.UpdateCount(count);
        LastEditDate = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (Status == newStatus)
        {
            return; 
        }
        
        var allowedTransitions = Status switch
        {
            OrderStatus.Created => new[] { OrderStatus.InProgress, OrderStatus.Cancelled },
            OrderStatus.InProgress => new[] { OrderStatus.Completed, OrderStatus.Cancelled },
            _ => Array.Empty<OrderStatus>()
        };

        if (!allowedTransitions.Contains(newStatus))
        {
            throw new InvalidOperationException($"Cannot change status from {Status} to {newStatus}"); 
        }
        
        Status = newStatus;
        LastEditDate = DateTime.UtcNow;
        UpdateTimestamp();
    }
}