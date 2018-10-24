using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace BRPLUSA.Core.Services
{
    public class LoggingService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void LogError(string msg, Exception innerException)
        {
            _logger.Error(innerException, msg);
        }

        public static void LogInfo(string msg)
        {
            _logger.Info(msg);
        }

        public static void LogWarning(string msg)
        {
            _logger.Warn(msg);
        }
    }
}
