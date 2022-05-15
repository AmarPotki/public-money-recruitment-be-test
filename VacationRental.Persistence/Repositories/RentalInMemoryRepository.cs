using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Persistence.Repositories
{
    public class RentalInMemoryRepository: RepositoryInMemoryBase<Rental>, IRentalRepository
    {
        public RentalInMemoryRepository(IDictionary<int, Rental> db) : base(db)
        {

        }

        public Task<bool> IsExistAsync(int rentalId)
        {
            return Task.FromResult( DB.Keys.Contains(rentalId));
        }
    }
}