using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;

namespace NannyServices.Application.Products.Queries;

public sealed record GetProductsPagedQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResultDto<ProductDto>>;