using FluentValidation;

namespace NannyServices.Application.Customers.Queries;

public sealed class SearchCustomersValidator : AbstractValidator<SearchCustomersQuery>
{
    public SearchCustomersValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotNull()
            .MaximumLength(100);
    }
}