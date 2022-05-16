using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Framework.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Queries;
using VacationRental.Application.ViewModels;
using VacationRental.Resources.Messages;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarQuery _calendarQuery;

        public CalendarController(ICalendarQuery calendarQuery)
        {
            _calendarQuery = calendarQuery;
        }

        [HttpGet]
        //[HttpGet("{rentalId:noZeroes}/{start:datetime}/{nights:noZeroes}")]
        [ProducesResponseType(typeof(CalendarViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CalendarViewModel>> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
               // return StatusCode(StatusCodes.Status406NotAcceptable);
                throw new ApplicationServiceException(Errors.NightsMustBePositive);


            var result = await _calendarQuery.GetCalendar(rentalId, start, nights);
            return result;
        }
    }
}
