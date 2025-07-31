using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Orders.Commands;

public sealed record CreateOrderCommand(CreateOrderDto Dto) : IRequest<OrderDto>;