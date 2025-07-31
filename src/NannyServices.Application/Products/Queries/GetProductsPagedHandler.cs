using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.DTOs.Common;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Products.Queries;

public sealed class GetProductsPagedHandler(IProductRepository repo)
    : IRequestHandler<GetProductsPagedQuery, PagedResultDto<ProductDto>>
{
    public async Task<PagedResultDto<ProductDto>> Handle(GetProductsPagedQuery request, CancellationToken cancellationToken)
    {
        var products = await repo.GetPagedAsync(request.Page, request.PageSize, cancellationToken);
        var total = await repo.CountAsync(cancellationToken: cancellationToken);

        return products.ToPagedDto(total, request.Page, request.PageSize, p => p.ToDto());
    }
}