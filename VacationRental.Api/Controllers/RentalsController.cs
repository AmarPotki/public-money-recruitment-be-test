using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Commands;
using VacationRental.Domain.Aggregates.RentalAggregate;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMediator _mediator;
        public RentalsController( IMediator mediator, IRentalRepository rentalRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            if(!await _rentalRepository.IsExistAsync(rentalId))
                throw new ApplicationException("Rental not found");
            var rental = await _rentalRepository.FirstAsync(rentalId);

            // we can use Automapper or Mapster
            return new RentalViewModel{Id = rental.Id,Units = rental.Units };
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
          var result= await _mediator.Send(model);
          // we can use Automapper or Mapster
          var resourceIdViewModel = new ResourceIdViewModel { Id = result.Id};
          return resourceIdViewModel;
        }
    }
}
