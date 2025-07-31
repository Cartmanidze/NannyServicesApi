using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomersPagedHandler(ICustomerRepository repo)
    : IRequestHandler<GetCustomersPagedQuery, PagedResultDto<CustomerDto>>
{
    public async Task<PagedResultDto<CustomerDto>> Handle(GetCustomersPagedQuery request, CancellationToken cancellationToken)
    {
        var customers = await repo.GetPagedAsync(request.Page, request.PageSize, cancellationToken);
        var totalCount = await repo.CountAsync(cancellationToken: cancellationToken);

        return customers.ToPagedDto(totalCount, request.Page, request.PageSize, c => c.ToDto());
    }
}