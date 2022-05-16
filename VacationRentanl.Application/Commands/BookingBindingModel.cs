using System;
using MediatR;
using VacationRental.Domain.Aggregates.BookingAggregate;

namespace VacationRental.Application.Commands
{
    public class BookingBindingModel:IRequest<Booking>
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }
      //  public DateTime EndDate => _startIgnoreTime.AddDays(Nights);
    }
}