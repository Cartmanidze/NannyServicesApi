namespace NannyServices.Application.DTOs;

public record AddressDto(
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode
);