using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Customers.Queries;

public sealed record SearchCustomersQuery(string SearchTerm) : IRequest<IEnumerable<CustomerDto>>;