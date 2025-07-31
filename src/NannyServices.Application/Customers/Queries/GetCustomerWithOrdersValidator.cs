using FluentValidation;

namespace NannyServices.Application.Customers.Queries;

public sealed class GetCustomerWithOrdersValidator : AbstractValidator<GetCustomerWithOrdersQuery>
{
    public GetCustomerWithOrdersValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}