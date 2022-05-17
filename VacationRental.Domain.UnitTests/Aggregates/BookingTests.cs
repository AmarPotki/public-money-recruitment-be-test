using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.UnitTests.Builders;
using Xunit;

namespace VacationRental.Domain.UnitTests.Aggregates
{
    public class BookingTests
    {
        [Fact]
        public void Create_Booking_Successfully()
        {
            var booking = new Booking(Constants.RentalIdOne, Constants.UnitIdOne, DateTime.Now, 2);
            
            booking.Start.Should().Be(DateTime.Now.Date);
            booking.RentalId.Should().Be(Constants.RentalIdOne);
            booking.UnitId.Should().Be(Constants.UnitIdOne);
            booking.Nights.Should().Be(2);
        }
    }
}
