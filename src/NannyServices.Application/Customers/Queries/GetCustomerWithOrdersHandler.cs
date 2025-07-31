using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomerWithOrdersHandler(ICustomerRepository repo)
    : IRequestHandler<GetCustomerWithOrdersQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerWithOrdersQuery request, CancellationToken cancellationToken)
    {
        var customer = await repo.GetByIdWithOrdersAsync(request.Id, cancellationToken);
        return customer?.ToDto();
    }
}