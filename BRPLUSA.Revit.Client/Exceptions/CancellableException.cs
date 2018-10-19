using System;

namespace BRPLUSA.Revit.Client.Exceptions
{
    public class CancellableException : Exception
    {
        public CancellableException() { }

        public CancellableException(string message): base(message) { }

        public CancellableException(string msg, Exception innerException): base(msg, innerException) { }
    }
}
