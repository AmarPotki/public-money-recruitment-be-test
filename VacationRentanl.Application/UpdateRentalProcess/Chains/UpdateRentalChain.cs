using System.Threading.Tasks;
using VacationRental.Application.UpdateRentalProcess.Configuration;

namespace VacationRental.Application.UpdateRentalProcess.Chains
{
    public class UpdateRentalChain : UpdateRentalProcessChain
    {
        public UpdateRentalChain(UpdateRentalProcessChain successor) : base(successor)
        {
        }

        public override async Task<bool> HandleRequest(ProcessRequestData request)
        {
            if(request.OriginalBookings.Count == 0)
            {
                return await UpdateRental(request);
            }

            return await Successor.HandleRequest(request);
        }
    }
}
