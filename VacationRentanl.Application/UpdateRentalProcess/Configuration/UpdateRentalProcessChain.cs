using Framework.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Application.UpdateRentalProcess.Configuration
{
    public abstract class UpdateRentalProcessChain
    {
        public UpdateRentalProcessChain(UpdateRentalProcessChain successor)
        {
            Successor = successor;
        }

        protected UpdateRentalProcessChain Successor { get; }

        public abstract Task<bool> HandleRequest(ProcessRequestData request);

        protected void ProcessNewChanges(ProcessRequestData requestData)
        {

            var bookings = requestData.OriginalBookings.Select(x => new
            {
                StartDate = x.Start.Date,
                EndDate = x.Start.AddDays(x.Nights + requestData.UpdateRentalCommand.PreparationTimeInDays),
                x.UnitId,
                x.Nights
            });
            var endDate = bookings.Max(c => c.EndDate);
            var days = (endDate - DateTime.Now.Date).TotalDays;

            for (var i = 0; i < days; i++)
            {
                var currentDate = DateTime.Now.Date.AddDays(i);

                var bookedUnits = bookings.Count(c => c.StartDate <= currentDate.Date && c.StartDate.AddDays(c.Nights) > currentDate);
                var inPreparationTimes = bookings.Count(c => c.StartDate.AddDays(c.Nights) <= currentDate &&
                                                             c.EndDate > currentDate);
                if (bookedUnits + inPreparationTimes > requestData.UpdateRentalCommand.Units)
                    throw new ApplicationServiceException(requestData.ErrorMessage);
            }

        }

        protected Task<bool> IncreaseRentalUnits(ProcessRequestData requestData,int count)
        {
            requestData.Rental.IncreaseUnits(count);
            requestData.RentalRepository.Update(requestData.Rental);
            return Task.FromResult(true);
        }

        protected Task<bool> DecreaseRentalUnits(ProcessRequestData requestData, int count)
        {
            requestData.Rental.DecreaseUnits(count);
            requestData.RentalRepository.Update(requestData.Rental);
            return Task.FromResult(true);
        }

        protected Task<bool> UpdatePreparationTimeInDays(ProcessRequestData requestData, int requestPreparationTimeInDays)
        {
            requestData.Rental.UpdatePreparationTimeInDays(requestPreparationTimeInDays);
            requestData.RentalRepository.Update(requestData.Rental);
            return Task.FromResult(true);
        }

        protected Task<bool> UpdateRental(ProcessRequestData requestData)
        {
            if (requestData.UpdateRentalCommand.Units - requestData.Rental.AvailableUnitsCount() > 0)
                IncreaseRentalUnits(requestData, requestData.UpdateRentalCommand.Units - requestData.Rental.AvailableUnitsCount());
            else DecreaseRentalUnits(requestData, requestData.Rental.AvailableUnitsCount() - requestData.UpdateRentalCommand.Units);

            requestData.Rental.UpdatePreparationTimeInDays(requestData.UpdateRentalCommand.PreparationTimeInDays);
            requestData.RentalRepository.Update(requestData.Rental);
            return Task.FromResult(true);
        }
    }
}
