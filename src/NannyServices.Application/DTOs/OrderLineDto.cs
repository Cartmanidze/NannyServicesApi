namespace NannyServices.Application.DTOs;

public record OrderLineDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Count,
    MoneyDto Price,
    MoneyDto TotalPrice
);