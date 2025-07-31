using MediatR;
using NannyServices.Domain.Enums;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Commands;

public sealed class DeleteOrderHandler(IUnitOfWork uow) : IRequestHandler<DeleteOrderCommand, bool>
{
    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.Orders.GetByIdWithDetailsAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return false;
        }
        
        if (order.Status != OrderStatus.Created)
        {
            throw new InvalidOperationException("Only orders in Created status can be deleted");
        }

        await uow.Orders.DeleteAsync(order, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}