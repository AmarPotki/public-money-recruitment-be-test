using MediatR;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Application.Commands
{
    public class RentalBindingModel : IRequest<Rental>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}