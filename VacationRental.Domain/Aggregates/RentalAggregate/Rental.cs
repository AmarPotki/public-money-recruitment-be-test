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

        public int UnitsCount() => _units.Count(c=>c.IsEnabled);
        private void AddUnits(int unitNumbers)
        {
            for (var i = 1; i <= unitNumbers; i++)
            {
                _units.Add(new Unit(i));
            }
        }

        public void UpdatePreparationTimeInDays(int preparationTimeInDays)
        {
            PreparationTimeInDays = preparationTimeInDays;
            //event

        }

        public void DecreaseUnits(int count)
        {
            var units = _units.Where(c=>c.IsEnabled).OrderByDescending(x => x.Id).Take(count);
            foreach (var unit in units)
            {
                unit.Disable();
                //fire event
            }
        }

        public void IncreaseUnits(int count)
        {
            var nextUnitNumber = _units.Max(c=>c.UnitNumber) +1;
            for (var i = nextUnitNumber; i <= count + nextUnitNumber; i++)
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
            IsEnabled = true;
        }

        public bool IsEnabled { get; private set; }
        public int UnitNumber { get;private set; }

        public void Disable() => IsEnabled = false;
    }
}
