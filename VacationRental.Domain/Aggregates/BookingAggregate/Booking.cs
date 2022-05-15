using System;
using Framework.Domain;

namespace VacationRental.Domain.Aggregates.BookingAggregate
{
    public class Booking: AggregateRoot
    {
        public Booking(int rentalId, DateTime start, int nights)
        {
            RentalId = rentalId;
            Start = start;
            Nights = nights;
        }

        public int RentalId { get;private set; }
        public DateTime Start { get;private set; }
        public int Nights { get;private set; }
    }
}