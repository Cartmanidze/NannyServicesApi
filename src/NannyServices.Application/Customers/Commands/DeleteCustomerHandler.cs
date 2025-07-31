using MediatR;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Customers.Commands;

public sealed class DeleteCustomerHandler(IUnitOfWork uow) : IRequestHandler<DeleteCustomerCommand, bool>
{
    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await uow.Customers.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
            return false;

        await uow.Customers.DeleteAsync(customer, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}