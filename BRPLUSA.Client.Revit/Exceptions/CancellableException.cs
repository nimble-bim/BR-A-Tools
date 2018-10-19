using System;

namespace BRPLUSA.Client.Revit.Exceptions
{
    public class CancellableException : Exception
    {
        public CancellableException() { }

        public CancellableException(string message): base(message) { }

        public CancellableException(string msg, Exception innerException): base(msg, innerException) { }
    }
}
