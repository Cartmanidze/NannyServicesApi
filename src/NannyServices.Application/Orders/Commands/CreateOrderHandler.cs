using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Entities;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Orders.Commands;

public sealed class CreateOrderHandler(IUnitOfWork uow) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await uow.Customers.GetByIdAsync(request.Dto.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new InvalidOperationException($"Customer with ID {request.Dto.CustomerId} not found");
        }

        var order = new Order(customer);
        await uow.Orders.AddAsync(order, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        
        var orderWithDetails = await uow.Orders.GetByIdWithDetailsAsync(order.Id, cancellationToken);
        return orderWithDetails!.ToDto();
    }
}