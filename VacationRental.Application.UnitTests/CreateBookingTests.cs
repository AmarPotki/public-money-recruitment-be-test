using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Framework.Exceptions;
using VacationRental.Application.UnitTests.Builders;
using VacationRental.Resources.Messages;
using Xunit;
using Constants = VacationRental.Domain.UnitTests.Builders.Constants;

namespace VacationRental.Application.UnitTests
{
    public class CreateBookingTests
    {

        [Fact]
        public async Task Must_Throw_An_Exception_If_rental_Not_Found()
        {
            
            var createBooking = new CreateBookingCommandBuilder().WithDefaultBook().Build();
            var handler =
                new CreateBookingCommandHandlerBuilder()
                    .WithNullRental()
                    .Build();

            Func<Task> task = () => handler.Handle(createBooking, CancellationToken.None);
           await task.Should().ThrowAsync<ApplicationServiceException>().WithMessage(Errors.RentalNotFound);
        }

       [Fact]
       public async Task Must_Throw_An_Exception_If_There_Is_No_Available_Unit()
       {
            var startDate = DateTime.Now;

            var createBooking = 
                new CreateBookingCommandBuilder()
                    .WithRentalIdAndStartDate(Constants.RentalIdOne, startDate)
                    .Build();

            var handler =
                new CreateBookingCommandHandlerBuilder()
                    .WithRentalWithThreeUnitsAndBookingWithThreeBooked(startDate)
                    .Build();

            Func<Task> task = () => 
                handler.Handle(createBooking, CancellationToken.None);

            await task.Should().ThrowAsync<ApplicationServiceException>()
                .WithMessage(Errors.NotAvailable);

        }


       [Fact]
       public async Task Create_Booking_Successfully()
       {
           var startDate = DateTime.Now;

           var createBooking =
               new CreateBookingCommandBuilder()
                   .WithRentalIdAndStartDate(Constants.RentalIdOne, startDate)
                   .Build();

           var handler =
               new CreateBookingCommandHandlerBuilder()
                   .WithRentalWithThreeUnitsAndBookingWithTwoBooked(startDate)
                   .Build();

          var booking=
              await handler.Handle(createBooking, CancellationToken.None);

          booking.Should().NotBe(null);
          booking.Nights.Should().Be(createBooking.Nights);
          booking.Start.Should().Be(createBooking.Start);
          booking.RentalId.Should().Be(createBooking.RentalId);

       }

    }
}
