using FluentValidation;

namespace NannyServices.Application.Orders.Commands;

public sealed class UpdateOrderLineValidator : AbstractValidator<UpdateOrderLineCommand>
{
    public UpdateOrderLineValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Dto.OrderLineId).NotEmpty();
        RuleFor(x => x.Dto.Count).GreaterThan(0);
    }
}