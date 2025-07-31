using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Products.Queries;

public sealed class GetProductHandler(IProductRepository repo) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await repo.GetByIdAsync(request.Id, cancellationToken);
        return product?.ToDto();
    }
}