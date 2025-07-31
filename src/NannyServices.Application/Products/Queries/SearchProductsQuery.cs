using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Products.Queries;

public sealed record SearchProductsQuery(string SearchTerm) : IRequest<IEnumerable<ProductDto>>;