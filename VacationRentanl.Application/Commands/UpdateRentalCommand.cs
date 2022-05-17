using MediatR;

namespace VacationRental.Application.Commands
{
    public class UpdateRentalCommand : IRequest<bool>
    {

        public int Units { get; set; }
        public int RentalId { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}