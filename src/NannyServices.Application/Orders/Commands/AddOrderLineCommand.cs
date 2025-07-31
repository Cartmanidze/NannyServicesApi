using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Orders.Commands;

public sealed record AddOrderLineCommand(Guid OrderId, AddOrderLineDto Dto) : IRequest<OrderDto?>;