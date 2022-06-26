using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Exceptions;
using MediatR;
using VacationRental.Application.Commands;
using VacationRental.Application.UpdateRentalProcess.Chains;
using VacationRental.Application.UpdateRentalProcess.Configuration;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Resources.Messages;

namespace VacationRental.Application.CommandHandlers
{
    public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand, bool>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        public UpdateRentalCommandHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }
        public async Task<bool> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.FirstAsync(request.RentalId, CancellationToken.None);
            
            if (rental is null)
                throw new ApplicationServiceException(Errors.RentalNotFound);

            if (rental.PreparationTimeInDays == request.PreparationTimeInDays && rental.AvailableUnitsCount() == request.Units)
                throw new ApplicationServiceException(Errors.NoChange);

            var originalBookings = await _bookingRepository
                .GetBookingsByRentalIdAndStartDate(request.RentalId, DateTime.Now);

            var updateChain = new UpdateRentalChain
                    (new UpdatePreparationTimeInDay
                    (new DecreaseUnits
                    (new IncreaseUnits
                    (new UpdateUnitsAndPreparationTimeInDay(null)))));

            var chainDataRequest = new ProcessRequestData
            {
                OriginalBookings = originalBookings,
                Rental = rental,
                RentalRepository = _rentalRepository,
                UpdateRentalCommand = request,
            };

            return await updateChain.HandleRequest(chainDataRequest);
        }
    }
}