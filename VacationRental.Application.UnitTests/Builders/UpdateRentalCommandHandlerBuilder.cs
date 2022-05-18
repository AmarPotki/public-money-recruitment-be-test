using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using VacationRental.Application.CommandHandlers;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Domain.UnitTests.Builders;

namespace VacationRental.Application.UnitTests.Builders
{
    public class UpdateRentalCommandHandlerBuilder
    {
        public Moq.Mock<IRentalRepository> RentalRepository { get; }
        public Moq.Mock<IBookingRepository> BookingRepository { get; }

        public UpdateRentalCommandHandlerBuilder()
        {
            RentalRepository =
                new Moq.Mock<IRentalRepository>();

            BookingRepository =
                new Mock<IBookingRepository>();

            UpdateRentalCommandHandler =
                new UpdateRentalCommandHandler(RentalRepository.Object, BookingRepository.Object);
        }

        public UpdateRentalCommandHandler UpdateRentalCommandHandler { get; }

        public UpdateRentalCommandHandlerBuilder WithNullRental()
        {
            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(() => null);

            return this;
        }
        public UpdateRentalCommandHandlerBuilder WithSameValues(int units,int preparationTime)
        {
            var rental = new RentalBuilder().WithUnitsAndPreparationTime(units, preparationTime).Build();

            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(rental);

            return this;
        }
        public UpdateRentalCommandHandlerBuilder WithListOfBookingsWithFullyBookedForRentalWithTwoUnitsAndOnePreparationTime(DateTime startDate)
        {
            var rental = new RentalBuilder().WithUnitsAndPreparationTime(Constants.TwoUnits, Constants.OnePreparation).Build();

            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(rental);
            var bookings = new List<Booking>
            {
                new Booking(rental.Id, rental.Units[0].Id, startDate, 1),
                new Booking(rental.Id, rental.Units[1].Id, startDate, 2),
                new Booking(rental.Id, rental.Units[0].Id, startDate.AddDays(1 + rental.PreparationTimeInDays), 2),
                new Booking(rental.Id, rental.Units[1].Id, startDate.AddDays(2 + rental.PreparationTimeInDays), 3),
            };
            BookingRepository.Setup(c => c.GetBookingsByRentalIdAndStartDate(rental.Id, It.IsAny<DateTime>())).ReturnsAsync(bookings);
            return this;
        }
        public UpdateRentalCommandHandlerBuilder WithListOfBookingsWithBookingsThatExtendByOneDayForRentalWithTwoUnitsAndOnePreparationTime(DateTime startDate)
        {
            var rental = new RentalBuilder().WithUnitsAndPreparationTime(Constants.TwoUnits, Constants.OnePreparation).Build();

            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(rental);
            var bookings = new List<Booking>
            {
                new Booking(rental.Id, rental.Units[0].Id, startDate, 1),
                new Booking(rental.Id, rental.Units[1].Id, startDate, 2),
                new Booking(rental.Id, rental.Units[0].Id, startDate.AddDays(2 + rental.PreparationTimeInDays), 2),
                new Booking(rental.Id, rental.Units[1].Id, startDate.AddDays(3 + rental.PreparationTimeInDays), 3),
            };
            BookingRepository.Setup(c => c.GetBookingsByRentalIdAndStartDate(rental.Id, It.IsAny<DateTime>())).ReturnsAsync(bookings);
            RentalRepository.Setup(c => c.Update(It.IsAny<Rental>())).Verifiable();
            return this;
        }
        public UpdateRentalCommandHandlerBuilder WithListOfBookingsWithFullyBookedForRentalWithTwoUnitsAndTwoPreparationTime(DateTime startDate)
        {
            var rental = new RentalBuilder().WithUnitsAndPreparationTime(Constants.TwoUnits, Constants.TwoPreparations).Build();

            RentalRepository
                .Setup(x => x.FirstAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(rental);
            var bookings = new List<Booking>
            {
                new Booking(rental.Id, rental.Units[0].Id, startDate, 1),
                new Booking(rental.Id, rental.Units[1].Id, startDate, 2),
                new Booking(rental.Id, rental.Units[0].Id, startDate.AddDays(1 + rental.PreparationTimeInDays), 2),
                new Booking(rental.Id, rental.Units[1].Id, startDate.AddDays(2 + rental.PreparationTimeInDays), 3),
            };
            BookingRepository.Setup(c => c.GetBookingsByRentalIdAndStartDate(rental.Id, It.IsAny<DateTime>())).ReturnsAsync(bookings);
            RentalRepository.Setup(c => c.Update(It.IsAny<Rental>())).Verifiable();

            return this;
        }
        public UpdateRentalCommandHandler Build()
        {
            return UpdateRentalCommandHandler;
        }
    }
}