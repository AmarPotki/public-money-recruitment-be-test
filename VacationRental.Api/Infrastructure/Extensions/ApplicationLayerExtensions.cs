using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Queries;
using VacationRental.Domain.Aggregates.BookingAggregate;
using VacationRental.Domain.Aggregates.RentalAggregate;
using VacationRental.Persistence.Repositories;

namespace VacationRental.Api.Infrastructure.Extensions
{
    public static class ApplicationLayerExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IDictionary<int, Rental>>(new Dictionary<int, Rental>());
            services.AddSingleton<IDictionary<int, Booking>>(new Dictionary<int, Booking>());
            services.AddTransient<IRentalRepository, RentalInMemoryRepository>();
            services.AddTransient<IBookingRepository, BookingInMemoryRepository>();
            services.AddTransient<ICalendarQuery, CalendarQuery>();

        }
    }
}