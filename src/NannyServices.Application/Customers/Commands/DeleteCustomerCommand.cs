using MediatR;

namespace NannyServices.Application.Customers.Commands;

public sealed record DeleteCustomerCommand(Guid Id) : IRequest<bool>;