using Framework.Domain;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public class Rental : AggregateRoot
    {
        public Rental(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public int Units { get; private set; }
        public int PreparationTimeInDays { get; private set; }
    }
}
