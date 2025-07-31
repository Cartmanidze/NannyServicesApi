using FluentValidation;

namespace NannyServices.Application.Orders.Commands;

public sealed class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}