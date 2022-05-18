using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Framework.Exceptions;
using Moq;
using VacationRental.Application.UnitTests.Builders;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Resources.Messages;
using Xunit;
using Constants = VacationRental.Domain.UnitTests.Builders.Constants;

namespace VacationRental.Application.UnitTests
{
    public class UpdateRentalTests
    {
        [Fact]
        public async Task Must_Throw_An_Exception_If_rental_Not_Found()
        {

            var updateRentalCommand = new UpdateRentalCommandBuilder().WithDefault().Build();
            var handler =
                new UpdateRentalCommandHandlerBuilder()
                    .WithNullRental()
                    .Build();

            Func<Task> task = () => handler.Handle(updateRentalCommand, CancellationToken.None);
            await task.Should().ThrowAsync<ApplicationServiceException>().WithMessage(Errors.RentalNotFound);
        }
        [Fact]
        public async Task Must_Throw_An_Exception_If_Units_And_PreparationTime_Are_The_Same()
        {
            
            var updateRentalCommand = new UpdateRentalCommandBuilder().
                WithRentalIdAndUnits(Constants.RentalIdOne,Constants.TwoUnits,Constants.OnePreparation).Build();
            var handler =
                new UpdateRentalCommandHandlerBuilder()
                    .WithSameValues(Constants.TwoUnits, Constants.OnePreparation)
                    .Build();

            Func<Task> task = () => handler.Handle(updateRentalCommand, CancellationToken.None);
            await task.Should().ThrowAsync<ApplicationServiceException>().WithMessage(Errors.NoChange);
        }  
        [Fact]
        public async Task Must_Throw_An_Exception_If_NewPreparationTime_IS_Not_Proper()
        {
            
            var updateRentalCommand = new UpdateRentalCommandBuilder().
                WithRentalIdAndUnits(Constants.RentalIdOne,Constants.TwoUnits,2).Build();
            var handler =
                new UpdateRentalCommandHandlerBuilder()
                    .WithListOfBookingsWithFullyBookedForRentalWithTwoUnitsAndOnePreparationTime(DateTime.Now)
                    .Build();

            Func<Task> task = () => handler.Handle(updateRentalCommand, CancellationToken.None);
            await task.Should().ThrowAsync<ApplicationServiceException>().WithMessage(Errors.NewPreparationTimeInDaysFails);
        }

        [Fact]
        public async Task Update_Rental_And_BookingList_When_NewPreparationTime_IS_Proper()
        {

            var updateRentalCommand = new UpdateRentalCommandBuilder().
                WithRentalIdAndUnits(Constants.RentalIdOne, Constants.TwoUnits, 2).Build();
            var updateRentalCommandHandlerBuilder =
                new UpdateRentalCommandHandlerBuilder();
            var handler =
                updateRentalCommandHandlerBuilder
                    .WithListOfBookingsWithBookingsThatExtendByOneDayForRentalWithTwoUnitsAndOnePreparationTime(DateTime.Now)
                    .Build();

         var result=await handler.Handle(updateRentalCommand, CancellationToken.None);
         updateRentalCommandHandlerBuilder.RentalRepository.Verify(c=>c.Update(It.IsAny<Rental>()),Times.Once);

         result.Should().Be(true);
        }

        [Fact]
        public async Task Must_Throw_An_Exception_If_Units_IS_Not_Proper()
        {

            var updateRentalCommand = new UpdateRentalCommandBuilder().
                WithRentalIdAndUnits(Constants.RentalIdOne, Constants.OneUnit, 1).Build();
            var handler =
                new UpdateRentalCommandHandlerBuilder()
                    .WithListOfBookingsWithFullyBookedForRentalWithTwoUnitsAndOnePreparationTime(DateTime.Now)
                    .Build();

            Func<Task> task = () => handler.Handle(updateRentalCommand, CancellationToken.None);
            await task.Should().ThrowAsync<ApplicationServiceException>().WithMessage(Errors.NewUnitsCountFails);
        }
        [Fact]
        public async Task Update_Rental_And_BookingList_When_NewUnits_IS_Bigger_Than_Current_Rental_Units()
        {

            var updateRentalCommand = new UpdateRentalCommandBuilder().
                WithRentalIdAndUnits(Constants.RentalIdOne, Constants.ThreeUnit, 1).Build();
            var updateRentalCommandHandlerBuilder = new UpdateRentalCommandHandlerBuilder();
            var handler =
                updateRentalCommandHandlerBuilder
                    .WithListOfBookingsWithFullyBookedForRentalWithTwoUnitsAndOnePreparationTime(DateTime.Now)
                    .Build();

            var result= await handler.Handle(updateRentalCommand, CancellationToken.None);
            updateRentalCommandHandlerBuilder.RentalRepository.Verify(c => c.Update(It.IsAny<Rental>()), Times.Once);
            result.Should().Be(true);
        }

        [Fact]
        public async Task Update_Rental_And_BookingList_When_NewUnits_IS_Bigger_Than_Current_Rental_Units_And_PreparationTime_Less_Than_Current_Rental_PreparationTime()
        {

            var updateRentalCommand = new UpdateRentalCommandBuilder().
                WithRentalIdAndUnits(Constants.RentalIdOne, Constants.ThreeUnit, 1).Build();
            var updateRentalCommandHandlerBuilder = new UpdateRentalCommandHandlerBuilder();
            var handler =
                updateRentalCommandHandlerBuilder
                    .WithListOfBookingsWithFullyBookedForRentalWithTwoUnitsAndTwoPreparationTime(DateTime.Now)
                    .Build();

            var result = await handler.Handle(updateRentalCommand, CancellationToken.None);
            updateRentalCommandHandlerBuilder.RentalRepository.Verify(c => c.Update(It.IsAny<Rental>()), Times.Exactly(2));
            result.Should().Be(true);
        }
    }
}