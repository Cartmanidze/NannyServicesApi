using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.DTOs.Reports;
using NannyServices.Domain.Entities;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Application.Mappings;

public static class EntityMappers
{
    private static AddressDto ToDto(this Address address)
    {
        return new AddressDto(
            address.Street,
            address.City,
            address.State,
            address.Country,
            address.PostalCode
        );
    }

    public static Address ToEntity(this AddressDto dto)
    {
        return new Address(dto.Street, dto.City, dto.State, dto.Country, dto.PostalCode);
    }

    private static MoneyDto ToDto(this Money money)
    {
        return new MoneyDto(money.Amount, money.Currency);
    }

    public static Money ToEntity(this MoneyDto dto)
    {
        return new Money(dto.Amount, dto.Currency);
    }
    
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.Name,
            customer.LastName,
            customer.FullName,
            customer.Address.ToDto(),
            customer.Photo,
            customer.CreatedAt,
            customer.UpdatedAt
        );
    }

    public static Customer ToEntity(this CreateCustomerDto dto)
    {
        return new Customer(
            dto.Name,
            dto.LastName,
            dto.Address.ToEntity(),
            dto.Photo
        );
    }
    
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Price.ToDto(),
            product.CreatedAt,
            product.UpdatedAt
        );
    }

    public static Product ToEntity(this CreateProductDto dto)
    {
        return new Product(dto.Name, dto.Price.ToEntity());
    }

    private static OrderLineDto ToDto(this OrderLine orderLine)
    {
        return new OrderLineDto(
            orderLine.Id,
            orderLine.ProductId,
            orderLine.Product.Name,
            orderLine.Count,
            orderLine.Price.ToDto(),
            orderLine.TotalPrice.ToDto()
        );
    }
    
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Customer.FullName,
            order.Status,
            order.TotalAmount.ToDto(),
            order.CreatedAt,
            order.LastEditDate,
            order.OrderLines.Select(ol => ol.ToDto())
        );
    }
    
    public static PagedResultDto<TDto> ToPagedDto<TEntity, TDto>(
        this IEnumerable<TEntity> items,
        int totalCount,
        int page,
        int pageSize,
        Func<TEntity, TDto> mapper)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return new PagedResultDto<TDto>(
            items.Select(mapper),
            totalCount,
            page,
            pageSize,
            totalPages
        );
    }
    
    public static CustomerReportDto ToReportDto(
        this Customer customer,
        DateTime startDate,
        DateTime endDate,
        int orderCount,
        Money totalAmount,
        ProductSummaryDto? mostOrderedProduct)
    {
        return new CustomerReportDto(
            customer.Id,
            customer.FullName,
            startDate,
            endDate,
            orderCount,
            totalAmount.ToDto(),
            mostOrderedProduct
        );
    }
}