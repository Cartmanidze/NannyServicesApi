using MediatR;

namespace NannyServices.Application.Orders.Commands;

public sealed record DeleteOrderCommand(Guid OrderId) : IRequest<bool>;