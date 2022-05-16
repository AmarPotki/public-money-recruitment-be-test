using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Behaviors;
using VacationRental.Application.CommandHandlers;
using VacationRental.Application.ViewModels;

namespace VacationRental.Api.Infrastructure.Extensions
{
    public static class MediatRExtensions
    {
        public static void AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateRentalCommandHandler));
            services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}