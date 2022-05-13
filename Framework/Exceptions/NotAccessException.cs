using System;

namespace Framework.Exceptions
{
    public class NotAccessException : Exception
    {
        public NotAccessException()
        { }

        public NotAccessException(string message)
            : base(message)
        { }

        public NotAccessException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}