using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Commands;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Resources.Messages;

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
        [Route("{bookingId:noZeroes}")]
        [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            if (!await _bookingRepository.IsExistAsync(bookingId))
                // it can be changed to standard status code 404
                //  return  NotFound("Booking not found");
                throw new ApplicationServiceException(Errors.BookingNotFound);

            var booking =await _bookingRepository.FirstAsync(bookingId);
            return Ok(new BookingViewModel
            {
                Id = booking.Id,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start
            });
        
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResourceIdViewModel>> Post(BookingBindingModel model)
        {

            var result = await _mediator.Send(model);

            var resourceIdViewModel = new ResourceIdViewModel { Id = result.Id };

            return Ok(resourceIdViewModel);
        }
    }
}
