using FluentValidation;

namespace NannyServices.Application.Orders.Queries;

public sealed class GetOrderValidator : AbstractValidator<GetOrderQuery>
{
    public GetOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}