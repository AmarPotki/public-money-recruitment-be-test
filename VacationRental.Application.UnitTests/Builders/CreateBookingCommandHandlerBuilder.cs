using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using VacationRental.Application.CommandHandlers;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Application.UnitTests.Builders
{
    public class CreateBookingCommandHandlerBuilder
    {
        public CreateBookingCommandHandlerBuilder()
        {
            RentalRepository =
                new Moq.Mock<IRentalRepository>();

            BookingRepository =
                new Mock<IBookingRepository>();

            BookingCommandHandler =
                new CreateBookingCommandHandler(RentalRepository.Object, BookingRepository.Object);
        }

        public CreateBookingCommandHandler BookingCommandHandler { get; }
        public Moq.Mock<IRentalRepository> RentalRepository { get; }
        public Moq.Mock<IBookingRepository> BookingRepository { get; }

        public CreateBookingCommandHandlerBuilder WithNullRental()
        {
            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            return this;
        }
        public CreateBookingCommandHandlerBuilder WithRentalWithThreeUnitsAndBookingWithThreeBooked(DateTime requestDate)
        {
            var rental =  new RentalBuilder().WithUnitsAndPreparationTime(3, 1).Build();
            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(rental);
            var books = new List<Booking>
            {
                new Booking(rental.Id, rental.Units[0].Id, requestDate, 1),
                new Booking(rental.Id, rental.Units[1].Id, requestDate, 1),
                new Booking(rental.Id, rental.Units[2].Id, requestDate, 1),
            };
            BookingRepository.Setup(x => x.GetBookingsByRentalIdAndStartDate(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(books);

            return this;
        }
        public CreateBookingCommandHandlerBuilder WithRentalWithThreeUnitsAndBookingWithTwoBooked(DateTime requestDate)
        {
            var rental = new RentalBuilder().WithUnitsAndPreparationTime(3, 1).Build();
            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(rental);
            var books = new List<Booking>
            {
                new Booking(rental.Id, rental.Units[0].Id, requestDate, 1),
                new Booking(rental.Id, rental.Units[1].Id, requestDate, 1),
        
            };
            BookingRepository.Setup(x => x.GetBookingsByRentalIdAndStartDate(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(books);
            var availableUnitId = rental.Units[2].Id;
            var booking = new Booking(rental.Id, availableUnitId, requestDate, 1);
            booking.SetId(1);
            
            BookingRepository.Setup(x => x.AddAsync(booking,CancellationToken.None)).ReturnsAsync(booking);

            return this;
        }
        public CreateBookingCommandHandler Build()
        {
            return BookingCommandHandler;
        }
    }
}
