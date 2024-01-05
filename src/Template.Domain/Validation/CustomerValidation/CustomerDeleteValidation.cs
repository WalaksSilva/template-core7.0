using FluentValidation;
using Template.Domain.Models;

namespace Template.Domain.Validation.CustomerValidation;

public class CustomerDeleteValidation : AbstractValidator<Customer>
{
    public CustomerDeleteValidation()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id não pode ser nulo");
    }
}
