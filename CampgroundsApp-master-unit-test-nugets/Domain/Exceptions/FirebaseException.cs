using System;

namespace Domain.Exceptions
{
    public class FirebaseException : Exception
    {
        public override string Message { get; }
        public int StatusCode { get; }

        public FirebaseException(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}