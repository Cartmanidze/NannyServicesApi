using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Customers.Commands;

public sealed class CreateCustomerHandler(IUnitOfWork uow) : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await uow.Customers.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}