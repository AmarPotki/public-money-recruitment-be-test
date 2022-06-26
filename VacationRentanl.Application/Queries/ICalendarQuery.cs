using System;
using System.Threading.Tasks;
using VacationRental.Application.ViewModels;

namespace VacationRental.Application.Queries
{
    public interface ICalendarQuery
    {
        Task<CalendarViewModel> GetCalendar(int rentalId, DateTime start, int nights);
    }
}