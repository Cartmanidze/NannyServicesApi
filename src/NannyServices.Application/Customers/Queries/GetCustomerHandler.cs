using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Domain.Repositories;
using NannyServices.Application.Mappings;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomerHandler(ICustomerRepository repo) : IRequestHandler<GetCustomerQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await repo.GetByIdAsync(request.Id, cancellationToken);
        return customer?.ToDto();
    }
}