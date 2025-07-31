using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrdersByStatusHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersByStatusQuery, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
    {
        var orders = await repo.GetByStatusAsync(request.Status, cancellationToken);
        return orders.Select(o => o.ToDto());
    }
}