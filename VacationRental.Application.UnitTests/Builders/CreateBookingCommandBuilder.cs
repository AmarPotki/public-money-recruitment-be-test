using System;
using VacationRental.Application.Commands;
using VacationRental.Domain.UnitTests.Builders;

namespace VacationRental.Application.UnitTests.Builders
{
    public class CreateBookingCommandBuilder
    {
        private BookingBindingModel _bindingModel;

        public CreateBookingCommandBuilder()
        {
           
        }

        public CreateBookingCommandBuilder WithDefaultBook()
        {
            _bindingModel = new BookingBindingModel
                { Nights = 1, RentalId = Constants.RentalIdOne, Start = DateTime.Now };
            return this;
        }

        public CreateBookingCommandBuilder WithRentalIdAndStartDate(int rentalId,DateTime start)
        {
            _bindingModel = new BookingBindingModel
                { Nights = 1, RentalId = rentalId, Start = start };
            return this;
        }
        public BookingBindingModel Build()
        {
            return _bindingModel;
        }
    }
}