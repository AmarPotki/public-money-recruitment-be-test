using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.BookingAggregate;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBookingRepository _bookingRepository;
        public BookingsController(IMediator mediator, IBookingRepository bookingRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            if (!await _bookingRepository.IsExistAsync(bookingId))
                // it can be changed to standard status code 404
                //  return  NotFound("Booking not found");
                throw new ApplicationException("Rental not found");

            var booking =await _bookingRepository.FirstAsync(bookingId);
            return new BookingViewModel
            {
                Id = booking.Id,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start
            };
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {

            var result = await _mediator.Send(model);

            var resourceIdViewModel = new ResourceIdViewModel { Id = result.Id };

            return resourceIdViewModel;
        }
    }
}
