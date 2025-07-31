using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Customers.Queries;

public sealed record GetCustomerWithOrdersQuery(Guid Id) : IRequest<CustomerDto?>;