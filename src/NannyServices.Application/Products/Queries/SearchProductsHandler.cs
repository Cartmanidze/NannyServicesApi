using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Products.Queries;

public sealed class SearchProductsHandler(IProductRepository repo)
    : IRequestHandler<SearchProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repo.SearchByNameAsync(request.SearchTerm, cancellationToken);
        return products.Select(p => p.ToDto());
    }
}