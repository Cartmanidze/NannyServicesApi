using MediatR;
using NannyServices.Application.DTOs;

namespace NannyServices.Application.Products.Queries;

public sealed record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;