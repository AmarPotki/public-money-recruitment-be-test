using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using VacationRental.Application.CommandHandlers;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Persistence.Repositories;
using Xunit;

namespace VacationRental.Application.UnitTests
{
    public class CreateRentalTests
    {
        [Fact]
        public async Task Add_Rental_To_InMemoryRepository()
        {


           var rentalRepository = new RentalInMemoryRepository( new Dictionary<int, Domain.Aggregates.RentalAggregate.Rental>(),null);

            var createRentalCommand = new RentalBindingModel{PreparationTimeInDays = 1,Units = 2};
            var handler = new CreateRentalCommandHandler(rentalRepository);
           var rental = await handler.Handle(createRentalCommand, CancellationToken.None);

           rental.Id.Should().BeGreaterThan(0);
           rental.AvailableUnitsCount().Should().Be(2);
           rental.PreparationTimeInDays.Should().Be(1);

        }
    }
}