using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.DTOs.Reports;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Entities;
using NannyServices.Domain.Repositories;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Application.Services;

public class CustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);
        return customer?.ToDto();
    }

    public async Task<CustomerDto?> GetByIdWithOrdersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdWithOrdersAsync(id, cancellationToken);
        return customer?.ToDto();
    }

    public async Task<PagedResultDto<CustomerDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var customers = await _unitOfWork.Customers.GetPagedAsync(page, pageSize, cancellationToken);
        var totalCount = await _unitOfWork.Customers.CountAsync(cancellationToken: cancellationToken);

        return customers.ToPagedDto(totalCount, page, pageSize, c => c.ToDto());
    }

    public async Task<IEnumerable<CustomerDto>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var customers = await _unitOfWork.Customers.SearchByNameAsync(searchTerm, cancellationToken);
        return customers.Select(c => c.ToDto());
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto, CancellationToken cancellationToken = default)
    {
        var customer = createCustomerDto.ToEntity();

        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToDto();
    }

    public async Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto updateCustomerDto, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);
        if (customer == null)
        {
            return null;
        }

        customer.UpdateName(updateCustomerDto.Name);
        customer.UpdateLastName(updateCustomerDto.LastName);
        customer.UpdateAddress(updateCustomerDto.Address.ToEntity());
        customer.UpdatePhoto(updateCustomerDto.Photo);

        await _unitOfWork.Customers.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return false;

        await _unitOfWork.Customers.DeleteAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<CustomerReportDto?> GetReportAsync(Guid customerId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
        {
            return null;
        }

        var orders = await _unitOfWork.Orders.GetByCustomerIdAndDateRangeAsync(customerId, startDate, endDate, cancellationToken);
        var ordersList = orders.ToList();

        if (!ordersList.Any())
        {
            return customer.ToReportDto(
                startDate,
                endDate,
                0,
                Money.Zero("USD"),
                null
            );
        }

        var totalAmount = ordersList.Aggregate(Money.Zero("USD"), (total, order) => total + order.TotalAmount);
        var mostOrderedProduct = GetMostOrderedProduct(ordersList);

        return customer.ToReportDto(
            startDate,
            endDate,
            ordersList.Count,
            totalAmount,
            mostOrderedProduct
        );
    }

    private static ProductSummaryDto? GetMostOrderedProduct(IEnumerable<Order> orders)
    {
        var productGroups = orders
            .SelectMany(o => o.OrderLines)
            .GroupBy(ol => ol.Product)
            .Select(g => new
            {
                Product = g.Key,
                TotalQuantity = g.Sum(ol => ol.Count)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefault();

        return productGroups != null
            ? new ProductSummaryDto(productGroups.Product.Id, productGroups.Product.Name, productGroups.TotalQuantity)
            : null;
    }
}