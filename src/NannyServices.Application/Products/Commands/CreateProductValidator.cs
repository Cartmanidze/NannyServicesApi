using FluentValidation;

namespace NannyServices.Application.Products.Commands;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Price.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.Price.Currency).NotEmpty();
    }
}