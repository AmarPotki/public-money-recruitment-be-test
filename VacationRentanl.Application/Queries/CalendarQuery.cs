﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Exceptions;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Resources.Messages;

namespace VacationRental.Application.Queries
{
    public class CalendarQuery : ICalendarQuery
    {
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IDictionary<int, Rental> _rentals;

        public CalendarQuery(IDictionary<int, Rental> rentals, IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public Task<CalendarViewModel> GetCalendar(int rentalId, DateTime start, int nights)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationServiceException(Errors.RentalNotFound);

            var rentalUnits = _rentals[rentalId].Units;
            var preparationTimeInDays = _rentals[rentalId].PreparationTimeInDays;
            var bookings = _bookings.Values.Where(x => x.RentalId == rentalId && x.Start<= start.AddDays(nights).Date).ToList();

            var calendar = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<UnitViewModel>()
                };

                var bookedUnits = bookings.Where(c => c.Start <= date.Date  && c.Start.AddDays(c.Nights) > date.Date);
                foreach (var bookedUnit in bookedUnits)
                {

                        date.Bookings.Add(new CalendarBookingViewModel
                            {Id = bookedUnit.Id, Unit = rentalUnits.First(x=>x.Id==bookedUnit.UnitId).UnitNumber });
                }

                var inPreparationTimes = bookings.Where(c => c.Start.AddDays(c.Nights) <= date.Date &&
                                                             c.Start.AddDays(c.Nights).AddDays(preparationTimeInDays) > date.Date);

                foreach (var booking in inPreparationTimes)
                {

                    date.PreparationTimes.Add(new UnitViewModel {Unit = rentalUnits.First(x => x.Id == booking.UnitId).UnitNumber });
                    
                }

                calendar.Dates.Add(date);
            }

            return Task.FromResult(calendar);
        }


    }
}