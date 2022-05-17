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
            var bookings = originalBookings.Select(x => new
            {
                StartDate = x.Start.Date,
                EndDate = x.Start.AddDays(x.Nights + rental.PreparationTimeInDays),
                x.UnitId,
                x.Nights
            });

            if (request.PreparationTimeInDays != rental.PreparationTimeInDays && request.Units == rental.UnitsCount())
            {
                if (request.PreparationTimeInDays - rental.PreparationTimeInDays > 0)
                {
                    var newBookings = bookings.Select(x => new
                    {
                        x.StartDate,
                        EndDate = x.EndDate.AddDays(request.PreparationTimeInDays - rental.PreparationTimeInDays),
                        x.UnitId
                    });
                    foreach (var newBooking in newBookings)
                    {
                        var count = bookings.Count(x => x.StartDate <= newBooking.EndDate);
                        if (count >= rental.UnitsCount())
                            throw new ApplicationServiceException(Errors.NewPreparationTimeInDaysFails);
                    }

                }
                else
                {
                    //it's ok
                }
            }
            else if (request.PreparationTimeInDays == rental.PreparationTimeInDays && request.Units != rental.UnitsCount())
            {
                if (request.Units - rental.UnitsCount() < 0)
                {
                    var endate = bookings.Max(c => c.EndDate);
                    var days = (endate - DateTime.Now.Date).TotalDays;

                    for (var i = 0; i < days; i++)
                    {
                        var currentDate = DateTime.Now.Date.AddDays(i);


                        var bookedUnits = bookings.Count(c => c.StartDate.AddDays(c.Nights) > currentDate);
                        var inPreparationTimes = bookings.Count(c => c.StartDate.AddDays(c.Nights) <= currentDate &&
                                                                     c.EndDate > currentDate);
                        if (bookedUnits + inPreparationTimes > request.Units)
                            throw new ApplicationServiceException(Errors.NewUnitsCountFails);
                    }
                }
            }
            else
            {
                // (request.PreparationTimeInDays != rental.PreparationTimeInDays && request.Units != rental.UnitsCount())
                if (request.Units - rental.UnitsCount() > 0 &&
                    request.PreparationTimeInDays - rental.PreparationTimeInDays < 0)
                {
                    //it's ok
                }
                else if (request.Units - rental.UnitsCount() > 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays > 0)
                {

                    var newBookings = bookings.Select(x => new
                    {
                        x.StartDate,
                        EndDate = x.EndDate.AddDays(request.PreparationTimeInDays),
                        x.UnitId
                    });
                    foreach (var newBooking in newBookings)
                    {
                        var count = bookings.Count(x => x.StartDate <= newBooking.EndDate);
                        if (count >= request.Units)
                            throw new ApplicationServiceException(Errors.NewPreparationTimeInDaysFails);
                    }
                }
                else if (request.Units - rental.UnitsCount() < 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays > 0)
                {
                    var newbookings = originalBookings.Select(x => new
                    {
                        StartDate = x.Start.Date,
                        EndDate = x.Start.AddDays(x.Nights + request.PreparationTimeInDays),
                        x.UnitId,
                        x.Nights
                    });
                    var endate = newbookings.Max(c => c.EndDate);
                    var days = (endate - DateTime.Now.Date).TotalDays;

                    for (var i = 0; i < days; i++)
                    {
                        var currentDate = DateTime.Now.Date.AddDays(i);


                        var bookedUnits = newbookings.Count(c => c.StartDate.AddDays(c.Nights) > currentDate);
                        var inPreparationTimes = newbookings.Count(c => c.StartDate.AddDays(c.Nights) <= currentDate &&
                                                                        c.EndDate > currentDate);
                        if (bookedUnits + inPreparationTimes > request.Units)
                            throw new ApplicationServiceException(Errors.NewUnitsCountFails);
                    }

                    var newBookings = bookings.Select(x => new
                    {
                        x.StartDate,
                        EndDate = x.EndDate.AddDays(request.PreparationTimeInDays),
                        x.UnitId
                    });
                    foreach (var newBooking in newBookings)
                    {
                        var count = bookings.Count(x => x.StartDate <= newBooking.EndDate);
                        if (count >= request.Units)
                            throw new ApplicationServiceException(Errors.NewPreparationTimeInDaysFails);
                    }
                }
                else
                {
                    // request.Units - rental.UnitsCount() < 0 && request.PreparationTimeInDays - rental.PreparationTimeInDays < 0
                    var newbookings = originalBookings.Select(x => new
                    {
                        StartDate = x.Start.Date,
                        EndDate = x.Start.AddDays(x.Nights + request.PreparationTimeInDays),
                        x.UnitId,
                        x.Nights
                    });
                    var endate = newbookings.Max(c => c.EndDate);
                    var days = (endate - DateTime.Now.Date).TotalDays;

                    for (var i = 0; i < days; i++)
                    {
                        var currentDate = DateTime.Now.Date.AddDays(i);


                        var bookedUnits = newbookings.Count(c => c.StartDate.AddDays(c.Nights) > currentDate);
                        var inPreparationTimes = newbookings.Count(c => c.StartDate.AddDays(c.Nights) <= currentDate &&
                                                                        c.EndDate > currentDate);
                        if (bookedUnits + inPreparationTimes > request.Units)
                            throw new ApplicationServiceException(Errors.NewUnitsCountFails);
                    }



                }

            }





            return true;
        }
    }
}