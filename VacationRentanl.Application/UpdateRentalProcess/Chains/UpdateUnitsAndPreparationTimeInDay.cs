using System.Threading.Tasks;
using VacationRental.Application.UpdateRentalProcess.Configuration;
using VacationRental.Resources.Messages;

namespace VacationRental.Application.UpdateRentalProcess.Chains
{
    public class UpdateUnitsAndPreparationTimeInDay : UpdateRentalProcessChain
    {
        public UpdateUnitsAndPreparationTimeInDay(UpdateRentalProcessChain successor) : base(successor)
        {
        }

        public override async Task<bool> HandleRequest(ProcessRequestData request)
        {
            if (request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount() > 0 &&
                request.UpdateRentalCommand.PreparationTimeInDays - request.Rental.PreparationTimeInDays < 0)
            {
                await IncreaseRentalUnits(request, request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount());
                await UpdatePreparationTimeInDays(request, request.UpdateRentalCommand.PreparationTimeInDays);
            }

            if (request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount() > 0 &&
                request.UpdateRentalCommand.PreparationTimeInDays - request.Rental.PreparationTimeInDays > 0)
            {
                request.ErrorMessage = Errors.NewPreparationTimeInDaysOrUnitsFails;
                ProcessNewChanges(request);
                await IncreaseRentalUnits(request, request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount());
                await UpdatePreparationTimeInDays(request, request.UpdateRentalCommand.PreparationTimeInDays);
            }

            if (request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount() < 0 &&
                request.UpdateRentalCommand.PreparationTimeInDays - request.Rental.PreparationTimeInDays > 0)
            {
                request.ErrorMessage = Errors.NewPreparationTimeInDaysOrUnitsFails;
                ProcessNewChanges(request);
                await DecreaseRentalUnits(request, request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount());
                await UpdatePreparationTimeInDays(request, request.UpdateRentalCommand.PreparationTimeInDays);
            }

            if (request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount() < 0 &&
                request.UpdateRentalCommand.PreparationTimeInDays - request.Rental.PreparationTimeInDays < 0)
            {
                request.ErrorMessage = Errors.NewPreparationTimeInDaysOrUnitsFails;
                ProcessNewChanges(request);
                await DecreaseRentalUnits(request, request.UpdateRentalCommand.Units - request.Rental.AvailableUnitsCount());
                await UpdatePreparationTimeInDays(request, request.UpdateRentalCommand.PreparationTimeInDays);
            }

            return true;
        }
    }
}
