using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Commands;

public sealed class UpdateOrderStatusHandler(IUnitOfWork uow) : IRequestHandler<UpdateOrderStatusCommand, OrderDto?>
{
    public async Task<OrderDto?> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.Orders.GetByIdWithDetailsAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.ChangeStatus(request.Dto.Status);
        await uow.Orders.UpdateAsync(order, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}