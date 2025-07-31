using FluentValidation;

namespace NannyServices.Application.Products.Queries;

public sealed class SearchProductsValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsValidator()
    {
        RuleFor(x => x.SearchTerm).NotNull().MaximumLength(100);
    }
}