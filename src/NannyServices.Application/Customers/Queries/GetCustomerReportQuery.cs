using MediatR;
using NannyServices.Application.DTOs.Reports;

namespace NannyServices.Application.Customers.Queries;

public sealed record GetCustomerReportQuery(Guid Id, DateTime StartDate, DateTime EndDate) : IRequest<CustomerReportDto?>;