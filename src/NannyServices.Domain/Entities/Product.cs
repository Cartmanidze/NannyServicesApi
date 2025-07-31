using NannyServices.Domain.Common;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public Money Price { get; private set; }

    /// <summary>
    /// // For EF Core
    /// </summary>
    private Product() { }

    public Product(string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        Name = name;
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        Name = name;
        UpdateTimestamp();
    }

    public void UpdatePrice(Money price)
    {
        Price = price ?? throw new ArgumentNullException(nameof(price));
        UpdateTimestamp();
    }
}