using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Domain.UnitTests.Builders;

namespace VacationRental.Application.UnitTests.Builders
{
    public class RentalBuilder
    {
        private Rental _rental;

        public RentalBuilder()
        {
            _rental = new Rental(1, 1);
        }

        public RentalBuilder WithUnitsAndPreparationTime(int units, int preparationTime)
        {
            _rental = new Rental(units, preparationTime);
            _rental.SetId(Constants.RentalIdOne);
            for (var i = 0; i < _rental.Units.Count; i++)
            {
                _rental.Units[i].SetId(i+1);
            }
            return this;
        }

        public Rental Build() => _rental;

    }
}
