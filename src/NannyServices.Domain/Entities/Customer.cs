using NannyServices.Domain.Common;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public Address Address { get; private set; }
    public string? Photo { get; private set; }

    private readonly List<Order> _orders = [];
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private Customer() { } // For EF Core

    public Customer(string name, string lastName, Address address, string? photo = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        }

        Name = name;
        LastName = lastName;
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Photo = photo;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }

        Name = name;
        UpdateTimestamp();
    }

    public void UpdateLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        }

        LastName = lastName;
        UpdateTimestamp();
    }

    public void UpdateAddress(Address address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        UpdateTimestamp();
    }

    public void UpdatePhoto(string? photo)
    {
        Photo = photo;
        UpdateTimestamp();
    }

    public string FullName => $"{Name} {LastName}";
}