using System;

namespace Framework.Exceptions
{
    /// <summary>
    /// Exception type for Application exceptions
    /// </summary>
    public class ApplicationServiceException : Exception
    {
        public ApplicationServiceException()
        { }

        public ApplicationServiceException(string message)
            : base(message)
        { }

        public ApplicationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}