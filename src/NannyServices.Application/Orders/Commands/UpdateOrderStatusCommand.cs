using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Orders.Commands;

public sealed record UpdateOrderStatusCommand(Guid OrderId, UpdateOrderStatusDto Dto) : IRequest<OrderDto?>;