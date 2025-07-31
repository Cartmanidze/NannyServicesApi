using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Customers.Commands;

public sealed record CreateCustomerCommand(CreateCustomerDto Dto) : IRequest<CustomerDto>;