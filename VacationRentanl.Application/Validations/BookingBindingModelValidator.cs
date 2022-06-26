using System;
using FluentValidation;
using VacationRental.Application.Commands;

namespace VacationRental.Application.Validations
{
    public class BookingValidator : AbstractValidator<BookingBindingModel>
    {
        public BookingValidator()
        {
            RuleFor(command => command.Nights)
                .GreaterThan(0)
                .WithMessage("Nigts must be positive");

            RuleFor(command => command.Start)
                .NotEmpty()
                .WithMessage("Start date can not be empty");

            RuleFor(command => command.Start)
                .GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("Start date must grater or equal today");

            RuleFor(command => command.RentalId)
                .NotEmpty();
        }
    }
}