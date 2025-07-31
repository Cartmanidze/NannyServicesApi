using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Orders.Commands;

public sealed record RemoveOrderLineCommand(Guid OrderId, Guid OrderLineId) : IRequest<OrderDto?>;