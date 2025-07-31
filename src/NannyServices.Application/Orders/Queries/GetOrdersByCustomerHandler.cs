using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrdersByCustomerHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByCustomerQuery, PagedResultDto<OrderDto>>
{
    public async Task<PagedResultDto<OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await repo.GetPagedByCustomerAsync(request.CustomerId, request.Page, request.PageSize, cancellationToken);
        var allCustomerOrders = await repo.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        var total = allCustomerOrders.Count();

        return orders.ToPagedDto(total, request.Page, request.PageSize, o => o.ToDto());
    }
}