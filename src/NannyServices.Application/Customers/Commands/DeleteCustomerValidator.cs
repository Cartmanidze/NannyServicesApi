using FluentValidation;

namespace NannyServices.Application.Customers.Commands;

public sealed class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}