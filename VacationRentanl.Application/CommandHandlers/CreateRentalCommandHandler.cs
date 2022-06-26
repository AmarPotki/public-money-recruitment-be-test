using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Application.CommandHandlers
{
    public class CreateRentalCommandHandler : IRequestHandler<RentalBindingModel, Rental>
    {
        private readonly IRentalRepository _rentalRepository;
        public CreateRentalCommandHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public async Task<Rental> Handle(RentalBindingModel request, CancellationToken cancellationToken)
        {
            var rental = new Rental(request.Units, request.PreparationTimeInDays);
            await _rentalRepository.AddAsync(rental, CancellationToken.None);

            return rental;
        }
    }
}