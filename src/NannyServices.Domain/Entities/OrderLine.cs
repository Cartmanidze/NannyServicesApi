using NannyServices.Domain.Common;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Domain.Entities;

public class OrderLine : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Count { get; private set; }
    public Money Price { get; private set; }
    public Guid OrderId { get; private set; }

    public Money TotalPrice => Price * Count;

    /// <summary>
    ///  For EF Core
    /// </summary>
    private OrderLine() { }

    public OrderLine(Product product, int count, Money price)
    {
        if (count <= 0)
        {
            throw new ArgumentException("Count must be greater than zero", nameof(count));
        }

        Product = product ?? throw new ArgumentNullException(nameof(product));
        ProductId = product.Id;
        Count = count;
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }

    public void UpdateCount(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentException("Count must be greater than zero", nameof(count));
        }

        Count = count;
        UpdateTimestamp();
    }

    public void UpdatePrice(Money price)
    {
        Price = price ?? throw new ArgumentNullException(nameof(price));
        UpdateTimestamp();
    }

    internal void SetOrderId(Guid orderId)
    {
        OrderId = orderId;
    }
}