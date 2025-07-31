namespace NannyServices.Application.DTOs;

public record MoneyDto(
    decimal Amount,
    string Currency
);