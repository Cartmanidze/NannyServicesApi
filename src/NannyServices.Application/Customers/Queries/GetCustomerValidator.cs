using FluentValidation;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomerValidator : AbstractValidator<GetCustomerQuery>
{
    public GetCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}