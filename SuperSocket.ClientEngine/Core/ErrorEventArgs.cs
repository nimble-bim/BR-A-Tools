using System;

namespace SuperSocket.ClientEngine.Core
{
    public class ErrorEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
