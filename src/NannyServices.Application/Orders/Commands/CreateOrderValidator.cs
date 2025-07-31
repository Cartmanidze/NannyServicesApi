using FluentValidation;

namespace NannyServices.Application.Orders.Commands;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Dto.CustomerId).NotEmpty();
    }
}