using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Exceptions;
using MediatR;
using VacationRental.Application.Commands;
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

            if (rental.PreparationTimeInDays == request.PreparationTimeInDays && rental.UnitsCount() == request.Units)
                throw new ApplicationServiceException(Errors.NoChange);
            var originalBookings = await _bookingRepository.GetBookingsByRentalIdAndStartDate(request.RentalId, DateTime.Now);
            if (originalBookings.Count == 0)
            {
                return await UpdateRental(rental, request);

            }


            if (request.PreparationTimeInDays != rental.PreparationTimeInDays && request.Units == rental.UnitsCount())
            {
                ProcessNewChanges(request, originalBookings, request.PreparationTimeInDays, Errors.NewPreparationTimeInDaysFails);

                return await UpdatePreparationTimeInDays(rental, request.PreparationTimeInDays);

            }

            if (request.PreparationTimeInDays == rental.PreparationTimeInDays && request.Units != rental.UnitsCount())
            {
                if (request.Units - rental.UnitsCount() < 0)
                {
                    ProcessNewChanges(request, originalBookings,
                        request.PreparationTimeInDays, Errors.NewUnitsCountFails);

                    return await DecreaseRentalUnits(rental, rental.UnitsCount() - request.Units);
                }
                return await IncreaseRentalUnits(rental, request.Units - rental.UnitsCount());


            }


            if (request.Units - rental.UnitsCount() > 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays < 0)
            {
                await IncreaseRentalUnits(rental, request.Units - rental.UnitsCount());
                await UpdatePreparationTimeInDays(rental, request.PreparationTimeInDays);

            }

            if (request.Units - rental.UnitsCount() > 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays > 0)
            {

                ProcessNewChanges(request, originalBookings, request.PreparationTimeInDays, Errors.NewPreparationTimeInDaysOrUnitsFails);
                await IncreaseRentalUnits(rental, request.Units - rental.UnitsCount());
                await UpdatePreparationTimeInDays(rental, request.PreparationTimeInDays);

            }

            if (request.Units - rental.UnitsCount() < 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays > 0)
            {
                ProcessNewChanges(request, originalBookings, request.PreparationTimeInDays, Errors.NewPreparationTimeInDaysOrUnitsFails);
                await DecreaseRentalUnits(rental, rental.UnitsCount() - request.Units);
                await UpdatePreparationTimeInDays(rental, request.PreparationTimeInDays);
            }

            if (request.Units - rental.UnitsCount() < 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays < 0)
            {
                ProcessNewChanges(request, originalBookings, request.PreparationTimeInDays, Errors.NewPreparationTimeInDaysOrUnitsFails);
                await DecreaseRentalUnits(rental, rental.UnitsCount() - request.Units);
                await UpdatePreparationTimeInDays(rental, request.PreparationTimeInDays);
            }


            return true;
        }

        private void ProcessNewChanges(UpdateRentalCommand request,
            List<Booking> originalBookings, int preparationTimeInDays, string message)
        {

            var bookings = originalBookings.Select(x => new
            {
                StartDate = x.Start.Date,
                EndDate = x.Start.AddDays(x.Nights + preparationTimeInDays),
                x.UnitId,
                x.Nights
            });
            var endDate = bookings.Max(c => c.EndDate);
            var days = (endDate - DateTime.Now.Date).TotalDays;

            for (var i = 0; i < days; i++)
            {
                var currentDate = DateTime.Now.Date.AddDays(i);

                var bookedUnits = bookings.Count(c => c.StartDate <= currentDate.Date && c.StartDate.AddDays(c.Nights) > currentDate);
                var inPreparationTimes = bookings.Count(c => c.StartDate.AddDays(c.Nights) <= currentDate &&
                                                             c.EndDate > currentDate);
                if (bookedUnits + inPreparationTimes > request.Units)
                    throw new ApplicationServiceException(message);
            }

        }

        private Task<bool> IncreaseRentalUnits(Rental rental, int count)
        {
            rental.IncreaseUnits(count);
            _rentalRepository.Update(rental);
            return Task.FromResult(true);
        }

        private Task<bool> DecreaseRentalUnits(Rental rental, int count)
        {
            rental.DecreaseUnits(count);
            _rentalRepository.Update(rental);
            return Task.FromResult(true);
        }

        private Task<bool> UpdatePreparationTimeInDays(Rental rental, int requestPreparationTimeInDays)
        {
            rental.UpdatePreparationTimeInDays(requestPreparationTimeInDays);
            _rentalRepository.Update(rental);
            return Task.FromResult(true);
        }

        private Task<bool> UpdateRental(Rental rental, UpdateRentalCommand request)
        {
            if (request.Units - rental.UnitsCount() > 0)
                IncreaseRentalUnits(rental, request.Units - rental.UnitsCount());
            else DecreaseRentalUnits(rental, rental.UnitsCount() - request.Units);

            rental.UpdatePreparationTimeInDays(request.PreparationTimeInDays);
            _rentalRepository.Update(rental);
            return Task.FromResult(true);
        }
    }
}