using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Customers.Commands;

public sealed record UpdateCustomerCommand(Guid Id, UpdateCustomerDto Dto) : IRequest<CustomerDto?>;