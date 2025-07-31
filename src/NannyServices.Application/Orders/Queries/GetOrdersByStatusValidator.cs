using FluentValidation;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrdersByStatusValidator : AbstractValidator<GetOrdersByStatusQuery>
{
    public GetOrdersByStatusValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}