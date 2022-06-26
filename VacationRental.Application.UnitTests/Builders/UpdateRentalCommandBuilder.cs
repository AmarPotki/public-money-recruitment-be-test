using VacationRental.Application.Commands;
using VacationRental.Domain.UnitTests.Builders;

namespace VacationRental.Application.UnitTests.Builders
{
    public class UpdateRentalCommandBuilder
    {
        private UpdateRentalCommand _bindingModel;

        public UpdateRentalCommandBuilder()
        {

        }

        public UpdateRentalCommandBuilder WithDefault()
        {
            _bindingModel = new UpdateRentalCommand
                {  RentalId = Constants.RentalIdOne, Units = Constants.UnitIdOne };
            return this;
        }

        public UpdateRentalCommandBuilder WithRentalIdAndUnits(int rentalId, int units,int preparationTime)
        {
            _bindingModel = new UpdateRentalCommand
                {  RentalId = rentalId, Units = units,PreparationTimeInDays = preparationTime};
            return this;
        }
        public UpdateRentalCommand Build()
        {
            return _bindingModel;
        }
    }
}