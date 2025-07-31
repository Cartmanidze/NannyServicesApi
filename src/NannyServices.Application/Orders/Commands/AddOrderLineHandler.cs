using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Commands;

public sealed class AddOrderLineHandler(IUnitOfWork uow) : IRequestHandler<AddOrderLineCommand, OrderDto?>
{
    public async Task<OrderDto?> Handle(AddOrderLineCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.Orders.GetByIdWithDetailsAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return null;
        }

        var product = await uow.Products.GetByIdAsync(request.Dto.ProductId, cancellationToken);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with ID {request.Dto.ProductId} not found");
        }

        if (request.Dto.Count <= 0)
        {
            throw new ArgumentException("Order line count must be greater than zero");
        }

        var orderLine = order.AddOrderLine(product, request.Dto.Count);
        uow.MarkAsAdded(orderLine);

        await uow.SaveChangesAsync(cancellationToken);

        var updated = await uow.Orders.GetByIdWithDetailsAsync(order.Id, cancellationToken);
        return updated!.ToDto();
    }
}