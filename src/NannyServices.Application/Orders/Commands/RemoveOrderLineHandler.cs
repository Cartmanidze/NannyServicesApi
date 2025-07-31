using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Commands;

public sealed class RemoveOrderLineHandler(IUnitOfWork uow) : IRequestHandler<RemoveOrderLineCommand, OrderDto?>
{
    public async Task<OrderDto?> Handle(RemoveOrderLineCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.Orders.GetByIdWithDetailsAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.RemoveOrderLine(request.OrderLineId);

        await uow.Orders.UpdateAsync(order, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}