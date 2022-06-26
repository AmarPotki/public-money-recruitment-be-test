using System.Threading.Tasks;
using VacationRental.Application.UpdateRentalProcess.Configuration;
using VacationRental.Resources.Messages;

namespace VacationRental.Application.UpdateRentalProcess.Chains
{
    public class UpdatePreparationTimeInDay : UpdateRentalProcessChain
    {
        public UpdatePreparationTimeInDay(UpdateRentalProcessChain successor) : base(successor)
        {
        }

        public override async Task<bool> HandleRequest(ProcessRequestData request)
        {
            if (request.UpdateRentalCommand.PreparationTimeInDays != request.Rental.PreparationTimeInDays
                && request.UpdateRentalCommand.Units == request.Rental.AvailableUnitsCount())
            {
                request.ErrorMessage = Errors.NewPreparationTimeInDaysFails;
                ProcessNewChanges(request);

                return await UpdatePreparationTimeInDays(request, request.UpdateRentalCommand.PreparationTimeInDays);
            }

            return await Successor.HandleRequest(request);
        }
    }

}
