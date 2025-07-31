using FluentValidation;

namespace NannyServices.Application.Products.Commands;

public sealed class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}