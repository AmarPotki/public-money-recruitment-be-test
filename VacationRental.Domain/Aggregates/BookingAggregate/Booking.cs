using System;
using Framework.Domain;

namespace VacationRental.Domain.Aggregates.BookingAggregate
{
    public class Booking: AggregateRoot
    {
        public Booking(int rentalId,int unitId, DateTime start, int nights)
        {
            RentalId = rentalId;
            UnitId = unitId;
            Start = start.Date;
            Nights = nights;
        }

        public int RentalId { get;private set; }
        public int UnitId { get; private set; }
        public DateTime Start { get;private set; }
        public int Nights { get;private set; }
    }
}