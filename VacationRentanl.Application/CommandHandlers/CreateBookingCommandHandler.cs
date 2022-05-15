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

namespace VacationRental.Application.CommandHandlers
{
    public class CreateBookingCommandHandler : IRequestHandler<BookingBindingModel, Booking>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        public CreateBookingCommandHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
           
        }

        public async Task<Booking> Handle(BookingBindingModel request, CancellationToken cancellationToken)
        {

            var rental = await _rentalRepository.FirstAsync(request.RentalId);
            if(rental is null)
                throw new ApplicationException("Rental not found");

            var bookings =await _bookingRepository.GetBookingsByRentalId(request.RentalId);
            if (bookings != null && bookings.Any())
            {
                var count = bookings.Count(x => (x.Start <= request.Start.Date 
                                                 && x.Start.AddDays(x.Nights).AddDays(rental.PreparationTimeInDays) > request.Start.Date) ||
                                                (x.Start < request.Start.AddDays(request.Nights).AddDays(rental.PreparationTimeInDays)
                                                 && x.Start.AddDays(x.Nights).AddDays(rental.PreparationTimeInDays) >= request.Start.AddDays(request.Nights).AddDays(rental.PreparationTimeInDays))||
                                                (x.Start > request.Start && x.Start.AddDays(x.Nights).AddDays(rental.PreparationTimeInDays) < request.Start.AddDays(request.Nights).AddDays(rental.PreparationTimeInDays)));


                if (count >= rental.Units)
                    throw new ApplicationServiceException("Not available");
            }
            var booking =new Booking( request.RentalId, request.Start, request.Nights);

           await _bookingRepository.AddAsync(booking,CancellationToken.None);

            return booking;
        }
    }
}