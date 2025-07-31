using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrdersPagedHandler(IOrderRepository repo)
    : IRequestHandler<GetOrdersPagedQuery, PagedResultDto<OrderDto>>
{
    public async Task<PagedResultDto<OrderDto>> Handle(GetOrdersPagedQuery request, CancellationToken cancellationToken)
    {
        var orders = await repo.GetPagedAsync(request.Page, request.PageSize, cancellationToken);
        var total = await repo.CountAsync(cancellationToken: cancellationToken);

        return orders.ToPagedDto(total, request.Page, request.PageSize, o => o.ToDto());
    }
}