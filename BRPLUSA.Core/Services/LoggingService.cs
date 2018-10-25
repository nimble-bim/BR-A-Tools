using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BRPLUSA.Core.Services
{
    public static class LoggingService
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static LoggingService()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("newlog.log"));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        public static void LogError(string msg, Exception innerException)
        {
            //_logger.Error(innerException, msg);
            Trace.WriteLine(msg + innerException.Message);
            if(innerException.InnerException != null)
                Trace.WriteLine($"InnerException: ${innerException?.InnerException?.Message}");
        }

        public static void LogInfo(string msg)
        {
            Trace.WriteLine(msg);
        }

        public static void LogWarning(string msg)
        {
            Trace.WriteLine(msg);
        }
    }

    public class WarningLogger : TraceListener
    {
        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}
