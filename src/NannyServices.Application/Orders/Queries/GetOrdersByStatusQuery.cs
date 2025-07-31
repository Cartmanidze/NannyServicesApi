using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Domain.Enums;

namespace NannyServices.Application.Orders.Queries;

public sealed record GetOrdersByStatusQuery(OrderStatus Status) : IRequest<IEnumerable<OrderDto>>;