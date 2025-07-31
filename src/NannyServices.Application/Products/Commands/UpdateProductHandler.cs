using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Products.Commands;

public sealed class UpdateProductHandler(IUnitOfWork uow) : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await uow.Products.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return null;
        }
        
        if (await uow.Products.ExistsByNameAsync(request.Dto.Name, request.Id, cancellationToken))
        {
            throw new InvalidOperationException($"Another product with name '{request.Dto.Name}' already exists");
        }

        product.UpdateName(request.Dto.Name);
        product.UpdatePrice(request.Dto.Price.ToEntity());

        await uow.Products.UpdateAsync(product, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return product.ToDto();
    }
}