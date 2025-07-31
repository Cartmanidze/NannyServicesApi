using MediatR;
using NannyServices.Application.DTOs.Reports;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Entities;
using NannyServices.Domain.Repositories;
using NannyServices.Domain.ValueObjects;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomerReportHandler(IUnitOfWork uow)
    : IRequestHandler<GetCustomerReportQuery, CustomerReportDto?>
{
    public async Task<CustomerReportDto?> Handle(GetCustomerReportQuery request, CancellationToken cancellationToken)
    {
        var customer = await uow.Customers.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
            return null;

        var orders = await uow.Orders.GetByCustomerIdAndDateRangeAsync(request.Id, request.StartDate, request.EndDate, cancellationToken);
        var list = orders.ToList();

        if (list.Count == 0)
        {
            return customer.ToReportDto(request.StartDate, request.EndDate, 0, Money.Zero("USD"), null);
        }

        var totalAmount = list.Aggregate(Money.Zero("USD"), (total, o) => total + o.TotalAmount);
        var mostOrdered = GetMostOrderedProduct(list);

        return customer.ToReportDto(request.StartDate, request.EndDate, list.Count, totalAmount, mostOrdered);
    }

    private static ProductSummaryDto? GetMostOrderedProduct(IEnumerable<Order> orders)
    {
        var group = orders.SelectMany(o => o.OrderLines)
            .GroupBy(l => l.Product)
            .Select(g => new { Product = g.Key, TotalQuantity = g.Sum(l => l.Count) })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefault();

        return group is null ? null : new ProductSummaryDto(group.Product.Id, group.Product.Name, group.TotalQuantity);
    }
}