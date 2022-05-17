using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Aggregates.BookingAggregate;

namespace VacationRental.Persistence.Repositories
{
    public class BookingInMemoryRepository : RepositoryInMemoryBase<Booking>, IBookingRepository
    {
        public BookingInMemoryRepository(IDictionary<int, Booking> db) : base(db)
        {

        }

        public Task<List<Booking>> GetBookingsByRentalIdAndStartDate(int rentalId, DateTime dateTime)
        {
            var results = DB.Where(x => x.Value.RentalId == rentalId && x.Value.Start >= dateTime.Date)
                .Select(x => x.Value)
                .ToList();
            return Task.FromResult(results);
        }

        public Task<bool> IsExistAsync(int id)
        {
            return Task.FromResult(DB.Keys.Contains(id));
        }

        public Task<Booking> GetByIdAsync(int id)
        {
            return Task.FromResult(DB[id]);
        }
    }
}