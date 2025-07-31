namespace NannyServices.Application.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string LastName,
    string FullName,
    AddressDto Address,
    string? Photo,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateCustomerDto(
    string Name,
    string LastName,
    AddressDto Address,
    string? Photo = null
);

public record UpdateCustomerDto(
    string Name,
    string LastName,
    AddressDto Address,
    string? Photo = null
);