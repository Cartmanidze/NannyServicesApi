using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;

namespace NannyServices.Application.Customers.Queries;

public sealed record GetCustomersPagedQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResultDto<CustomerDto>>;