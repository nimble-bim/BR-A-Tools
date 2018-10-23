using System;

namespace BRPLUSA.Revit.Core.Exceptions
{
    public class CancellableException : Exception
    {
        public CancellableException() { }

        public CancellableException(string message): base(message) { }

        public CancellableException(string msg, Exception innerException): base(msg, innerException) { }
    }

    public class SpaceCreationException : Exception
    {
        public SpaceCreationException() { }

        public SpaceCreationException(string message) : base(message) { }

        public SpaceCreationException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
