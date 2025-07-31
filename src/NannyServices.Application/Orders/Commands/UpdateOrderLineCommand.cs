using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Orders.Commands;

public sealed record UpdateOrderLineCommand(Guid OrderId, UpdateOrderLineDto Dto) : IRequest<OrderDto?>;