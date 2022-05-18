using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Application.UpdateRentalProcess.Configuration
{
    public class ProcessRequestData
    {
        public Rental Rental { get; set; }
        public UpdateRentalCommand UpdateRentalCommand { get; set; }
        public List<Booking> OriginalBookings { get; set; }
        public string ErrorMessage { get; set; }
        public IRentalRepository RentalRepository { get; set; }
    }
}
