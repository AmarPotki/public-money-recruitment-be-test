using FluentValidation;
using VacationRental.Application.Commands;

namespace VacationRental.Application.Validations
{
    public class CreateRentalValidator : AbstractValidator<RentalBindingModel>
    {
        public CreateRentalValidator()
        {
            RuleFor(command => command.Units)
                .GreaterThan(0)
                .WithMessage("Units must be greater than zero!");

            RuleFor(command => command.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Units must be greater or equal to than zero!");
        }
    }
}