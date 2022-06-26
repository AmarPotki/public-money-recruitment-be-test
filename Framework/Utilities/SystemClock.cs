using System;

namespace Framework.Utilities
{
    public class SystemClock : IClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}