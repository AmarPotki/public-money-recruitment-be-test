using System;

namespace Framework.Helpers
{
    public static class Extensions
    {
        public static string GetException(this Exception exception)
        {
            string ExceptionMessage = exception.Message + "\r\n";
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                ExceptionMessage += exception.Message + "\r\n";
            }

            return ExceptionMessage;
        }

   
       

        public static DateTime? GetBeginningOfTheMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);
        public static DateTime GetBeginningOfTheWeek(this DateTime today)
        {
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return today;
                case DayOfWeek.Sunday:
                    return today.AddDays(-1);
                case DayOfWeek.Monday:
                    return today.AddDays(-2);
                case DayOfWeek.Tuesday:
                    return today.AddDays(-3);
                case DayOfWeek.Wednesday:
                    return today.AddDays(-4);
                case DayOfWeek.Thursday:
                    return today.AddDays(-5);
                case DayOfWeek.Friday:
                    return today.AddDays(-6);
            }

            return default;
        }
    }

}