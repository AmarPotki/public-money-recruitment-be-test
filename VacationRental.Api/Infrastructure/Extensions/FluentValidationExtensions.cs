using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.CommandHandlers;

namespace VacationRental.Api.Infrastructure.Extensions
{
    public static class FluentValidationExtensions
    {
        public static void AddCustomFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateRentalCommandHandler).Assembly);

        }
    }
}
