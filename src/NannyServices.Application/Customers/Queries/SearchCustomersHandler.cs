using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Customers.Queries;

public sealed class SearchCustomersHandler(ICustomerRepository repo)
    : IRequestHandler<SearchCustomersQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await repo.SearchByNameAsync(request.SearchTerm, cancellationToken);
        return customers.Select(c => c.ToDto());
    }
}