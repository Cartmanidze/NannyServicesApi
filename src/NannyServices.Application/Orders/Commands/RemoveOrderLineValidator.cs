using FluentValidation;

namespace NannyServices.Application.Orders.Commands;

public sealed class RemoveOrderLineValidator : AbstractValidator<RemoveOrderLineCommand>
{
    public RemoveOrderLineValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.OrderLineId).NotEmpty();
    }
}