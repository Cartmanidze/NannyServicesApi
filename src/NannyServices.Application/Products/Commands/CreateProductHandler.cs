using MediatR;
using NannyServices.Application.DTOs;
using NannyServices.Application.Mappings;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Products.Commands;

public sealed class CreateProductHandler(IUnitOfWork uow) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await uow.Products.ExistsByNameAsync(request.Dto.Name, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Product with name '{request.Dto.Name}' already exists");
        }

        var entity = request.Dto.ToEntity();
        await uow.Products.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }
}