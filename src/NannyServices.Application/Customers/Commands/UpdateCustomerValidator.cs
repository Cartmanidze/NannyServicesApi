using FluentValidation;

namespace NannyServices.Application.Customers.Commands;

public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Dto.Address)
            .SetValidator(new AddressDtoValidator());
    }
}
