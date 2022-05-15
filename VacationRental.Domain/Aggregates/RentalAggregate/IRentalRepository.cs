using System.Threading.Tasks;
using Framework.Domain;

namespace VacationRental.Domain.Aggregates.RentalAggregate
{
    public interface IRentalRepository:IRepository<Rental>
    {
        Task<bool> IsExistAsync(int rentalId);
    }
}