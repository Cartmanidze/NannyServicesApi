using FluentValidation;

namespace NannyServices.Application.Orders.Commands;

public sealed class AddOrderLineValidator : AbstractValidator<AddOrderLineCommand>
{
    public AddOrderLineValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Dto.ProductId).NotEmpty();
        RuleFor(x => x.Dto.Count).GreaterThan(0);
    }
}