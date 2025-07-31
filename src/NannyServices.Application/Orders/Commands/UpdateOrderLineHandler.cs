using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Commands;

public sealed class UpdateOrderLineHandler(IUnitOfWork uow) : IRequestHandler<UpdateOrderLineCommand, OrderDto?>
{
    public async Task<OrderDto?> Handle(UpdateOrderLineCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.Orders.GetByIdWithDetailsAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        if (request.Dto.Count <= 0)
        {
            throw new ArgumentException("Order line count must be greater than zero");
        }

        order.UpdateOrderLineCount(request.Dto.OrderLineId, request.Dto.Count);

        await uow.Orders.UpdateAsync(order, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}