using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Domain;

namespace VacationRental.Domain.Aggregates.BookingAggregate
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<List<Booking>> GetBookingsByRentalIdAndStartDate(int requestRentalId, DateTime dateTime);
        Task<bool> IsExistAsync(int bookingId);
        Task<Booking> GetByIdAsync(int bookingId);
    }
}