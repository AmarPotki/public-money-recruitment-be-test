using System.Threading.Tasks;
using VacationRental.Application.UpdateRentalProcess.Configuration;
using VacationRental.Resources.Messages;

namespace VacationRental.Application.UpdateRentalProcess.Chains
{
    public class DecreaseUnits : UpdateRentalProcessChain
    {
        public DecreaseUnits(UpdateRentalProcessChain successor) : base(successor)
        {
        }

        public override async Task<bool> HandleRequest(ProcessRequestData request)
        {
            if (request.UpdateRentalCommand.PreparationTimeInDays == request.Rental.PreparationTimeInDays &&
                request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount() < 0)
            {
                request.ErrorMessage = Errors.NewUnitsCountFails;
                ProcessNewChanges(request);

                return await DecreaseRentalUnits(request, request.Rental.AvailableUnitsCount() - request.UpdateRentalCommand.Units);
            }

            return await Successor.HandleRequest(request);
        }
    }
}
