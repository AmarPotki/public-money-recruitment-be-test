using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Framework.Exceptions;
using MediatR;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Resources.Messages;

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

    public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand, bool>
    {
        private readonly IRentalRepository _rentalRepository;
        public UpdateRentalCommandHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public async Task<bool> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.FirstAsync(request.RentalId, CancellationToken.None);
            if (rental is null)
                throw new ApplicationServiceException(Errors.RentalNotFound);

            await _rentalRepository.AddAsync(rental, CancellationToken.None);

            return true;
        }
    }
}