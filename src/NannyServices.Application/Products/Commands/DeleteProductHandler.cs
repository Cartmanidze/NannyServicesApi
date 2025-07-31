using MediatR;
using NannyServices.Domain.Repositories;

namespace NannyServices.Application.Products.Commands;

public sealed class DeleteProductHandler(IUnitOfWork uow) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await uow.Products.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return false;

        await uow.Products.DeleteAsync(product, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}