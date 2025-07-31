using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Customers.Queries;

public sealed record GetCustomerQuery(Guid Id) : IRequest<CustomerDto?>;