using System.Runtime.Serialization;
using MediatR;

namespace VacationRental.Application.Commands
{
    public class UpdateRentalCommand : IRequest<bool>
    {

        public int Units { get; set; }
        [IgnoreDataMember]
        public int RentalId { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}