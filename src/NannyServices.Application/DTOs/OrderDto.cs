using NannyServices.Domain.Enums;

namespace NannyServices.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    OrderStatus Status,
    MoneyDto TotalAmount,
    DateTime CreatedAt,
    DateTime LastEditDate,
    IEnumerable<OrderLineDto> OrderLines
);

public record CreateOrderDto(
    Guid CustomerId
);

public record UpdateOrderStatusDto(
    OrderStatus Status
);

public record AddOrderLineDto(
    Guid ProductId,
    int Count
);

public record UpdateOrderLineDto(
    Guid OrderLineId,
    int Count
);