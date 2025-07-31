using NannyServices.Application.DTOs;
using NannyServices.Domain.Entities;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Api.IntegrationTests.Helpers;

public static class TestDataFactory
{
    public static CreateCustomerDto CreateValidCustomerDto(
        string name = "John",
        string lastName = "Doe",
        string street = "123 Main St",
        string city = "Anytown",
        string state = "State",
        string postalCode = "12345",
        string country = "USA")
    {
        var address = new AddressDto(street, city, state, postalCode, country);
        return new CreateCustomerDto(name, lastName, address);
    }

    public static UpdateCustomerDto CreateValidUpdateCustomerDto(
        string name = "Jane",
        string lastName = "Smith",
        string street = "456 Oak St",
        string city = "New City",
        string state = "New State",
        string postalCode = "67890",
        string country = "USA")
    {
        var address = new AddressDto(street, city, state, postalCode, country);
        return new UpdateCustomerDto(name, lastName, address);
    }

    public static CreateProductDto CreateValidProductDto(
        string name = "Test Product",
        decimal amount = 29.99m,
        string currency = "USD")
    {
        var money = new MoneyDto(amount, currency);
        return new CreateProductDto(name, money);
    }

    public static UpdateProductDto CreateValidUpdateProductDto(
        string name = "Updated Product",
        decimal amount = 39.99m,
        string currency = "USD")
    {
        var money = new MoneyDto(amount, currency);
        return new UpdateProductDto(name, money);
    }

    public static CreateOrderDto CreateValidOrderDto(Guid customerId)
    {
        return new CreateOrderDto(customerId);
    }

    public static AddOrderLineDto CreateValidAddOrderLineDto(
        Guid productId,
        int quantity = 2)
    {
        return new AddOrderLineDto(productId, quantity);
    }

    public static UpdateOrderLineDto CreateValidUpdateOrderLineDto(int quantity = 3)
    {
        return new UpdateOrderLineDto(Guid.NewGuid(), quantity);
    }

    public static Customer CreateCustomerEntity(
        string name = "John",
        string lastName = "Doe",
        string street = "123 Main St",
        string city = "Anytown",
        string state = "State",
        string postalCode = "12345",
        string country = "USA")
    {
        var address = new Address(street, city, state, country, postalCode);
        return new Customer(name, lastName, address);
    }

    public static Product CreateProductEntity(
        string name = "Test Product",
        decimal amount = 29.99m,
        string currency = "USD")
    {
        var money = new Money(amount, currency);
        return new Product(name, money);
    }
    
    public static CreateCustomerDto CreateInvalidCustomerDto()
    {
        var address = new AddressDto("", "", "", "", "");
        return new CreateCustomerDto("", "", address);
    }

    public static CreateProductDto CreateInvalidProductDto()
    {
        var money = new MoneyDto(-10m, "");
        return new CreateProductDto("", money);
    }

    public static CreateOrderDto CreateInvalidOrderDto()
    {
        return new CreateOrderDto(Guid.Empty);
    }

    public static AddOrderLineDto CreateInvalidAddOrderLineDto()
    {
        return new AddOrderLineDto(Guid.Empty, 0);
    }
} 