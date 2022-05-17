using System.Collections.Generic;
using System.Linq;
using Framework.Domain;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class Rental : AggregateRoot
    {
        public Rental(int units,int preparationTimeInDays)
        {

            PreparationTimeInDays = preparationTimeInDays;
            AddUnits(units);
        }

        private readonly List<Unit> _units = new List<Unit>();
        public IReadOnlyList<Unit> Units => _units;
        public int PreparationTimeInDays { get; private set; }

        public void AddUnit()
        {
            var unitNumber = _units.Any() ? _units.Max(c => c.UnitNumber) + 1 : 1;
            _units.Add(new Unit(unitNumber));
        }

        public int UnitsCount() => _units.Count;
        private void AddUnits(int unitNumbers)
        {
            for (var i = 1; i <= unitNumbers; i++)
            {
                _units.Add(new Unit(i));
            }
        }
    }

    public class Unit : Entity
    {
        public Unit(int unitNumber)
        {
            UnitNumber = unitNumber;
        }

        public int UnitNumber { get;private set; }
    }
}
