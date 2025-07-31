using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Customers.Commands;

public sealed class UpdateCustomerHandler(IUnitOfWork uow) : IRequestHandler<UpdateCustomerCommand, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await uow.Customers.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        customer.UpdateName(request.Dto.Name);
        customer.UpdateLastName(request.Dto.LastName);
        customer.UpdateAddress(request.Dto.Address.ToEntity());
        customer.UpdatePhoto(request.Dto.Photo);

        await uow.Customers.UpdateAsync(customer, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return customer.ToDto();
    }
}