using System;
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

            var rental = await _rentalRepository.FirstAsync(request.RentalId,CancellationToken.None);
            if(rental is null)
                throw new ApplicationServiceException(Errors.RentalNotFound);

            var bookings =(await _bookingRepository.GetBookingsByRentalId(request.RentalId)).Select(x=> new
            {
                StartDate = x.Start.Date,
                EndDate=x.Start.AddDays(x.Nights + rental.PreparationTimeInDays)
            });
            //real end date
            var requestEndDate = request.Start.AddDays(request.Nights+ rental.PreparationTimeInDays);
            if (bookings.Any())
            {
                var count = bookings.Count(c=>(c.StartDate <= request.Start && request.Start <= c.EndDate) ||
                                           (requestEndDate >= c.StartDate && requestEndDate <= c.EndDate) ||
                                           (request.Start <= c.StartDate && requestEndDate >= c.EndDate));


                if (count >= rental.Units)
                    throw new ApplicationServiceException(Errors.NotAvailable);
            }
            var booking =new Booking( request.RentalId, request.Start, request.Nights);

           await _bookingRepository.AddAsync(booking,CancellationToken.None);

            return booking;
        }
    }
}