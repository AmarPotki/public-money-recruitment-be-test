using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Commands;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Resources.Messages;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMediator _mediator;
        public RentalsController(IMediator mediator, IRentalRepository rentalRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        [ProducesResponseType(typeof(RentalViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            if (!await _rentalRepository.IsExistAsync(rentalId))
                // return NotFound("Rental not found");
                throw new ApplicationServiceException(Errors.RentalNotFound);
            var rental = await _rentalRepository.FirstAsync(rentalId);

            // we can use Automapper or Mapster
            return new RentalViewModel { Id = rental.Id, Units = rental.UnitsCount()  };
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResourceIdViewModel>> Post(RentalBindingModel model)
        {
            var result = await _mediator.Send(model);
            // we can use Automapper or Mapster
            var resourceIdViewModel = new ResourceIdViewModel { Id = result.Id };
            return resourceIdViewModel;
        }
    }
}
