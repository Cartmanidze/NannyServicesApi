using FluentValidation;

namespace NannyServices.Application.Orders.Commands;

public sealed class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Dto.Status).IsInEnum();
    }
}