using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Orders.Queries;

public sealed record GetOrderQuery(Guid Id) : IRequest<OrderDto?>;