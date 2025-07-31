using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;

namespace NannyServices.Application.Orders.Queries;

public sealed record GetOrdersByCustomerQuery(Guid CustomerId, int Page = 1, int PageSize = 10) : IRequest<PagedResultDto<OrderDto>>;