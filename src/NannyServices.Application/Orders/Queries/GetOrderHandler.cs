using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrderHandler(IOrderRepository repo) : IRequestHandler<GetOrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await repo.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        return order?.ToDto();
    }
}