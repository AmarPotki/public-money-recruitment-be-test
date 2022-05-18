using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using VacationRental.Domain.Aggregates.RentalAggregate;
using Xunit;

namespace VacationRental.Domain.UnitTests.Aggregates
{

    public class RentalTests
    {
        private readonly Rental _rental;

        public RentalTests()
        {
            _rental = new Rental(3, 1);
        }
        [Fact]
        public void Create_Rental_Successfully()
        {
            _rental.PreparationTimeInDays.Should().Be(1);
            _rental.AvailableUnitsCount().Should().Be(3);
            _rental.Units.Should().OnlyHaveUniqueItems();
        }
        [Fact]
        public void UnitNumber_Must_Be_Sequential()
        {
            var expectedList = new[] { 1, 2, 3 };

            _rental.PreparationTimeInDays.Should().Be(1);
            _rental.AvailableUnitsCount().Should().Be(3);
            _rental.Units.Select(x => x.UnitNumber).Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public void Increase_Units()
        {
            var units = _rental.AvailableUnitsCount();
            _rental.IncreaseUnits(3);
            _rental.AvailableUnitsCount().Should().Be(units +3);
        }
        [Fact]
        public void Decrease_Units()
        {
            var units = _rental.AvailableUnitsCount();
            _rental.DecreaseUnits(2);
            _rental.AvailableUnitsCount().Should().Be(units - 2);
           
        }
        [Fact]
        public void Decrease_Units_Means_DisableUnits()
        {
            var units = _rental.AvailableUnitsCount();
            _rental.DecreaseUnits(2);
            _rental.AvailableUnitsCount().Should().Be(units - 2);
            _rental.Units.Count.Should().Be(units);
        }
    }
}