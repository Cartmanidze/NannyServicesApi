namespace NannyServices.Application.DTOs.Reports;

public record CustomerReportDto(
    Guid CustomerId,
    string CustomerName,
    DateTime StartDate,
    DateTime EndDate,
    int OrderCount,
    MoneyDto TotalAmount,
    ProductSummaryDto? MostOrderedProduct
);

public record ProductSummaryDto(
    Guid ProductId,
    string ProductName,
    int TotalQuantity
);