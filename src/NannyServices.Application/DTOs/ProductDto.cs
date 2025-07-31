namespace NannyServices.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    MoneyDto Price,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateProductDto(
    string Name,
    MoneyDto Price
);

public record UpdateProductDto(
    string Name,
    MoneyDto Price
);