using Framework.Domain;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
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