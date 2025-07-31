using FluentValidation;

namespace NannyServices.Application.Products.Commands;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Price.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.Price.Currency).NotEmpty();
    }
}