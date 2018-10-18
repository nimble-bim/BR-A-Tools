using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Exceptions
{
    public class CancellableException : Exception
    {
        public CancellableException() { }

        public CancellableException(string message): base(message) { }

        public CancellableException(string msg, Exception innerException): base(msg, innerException) { }
    }
}
