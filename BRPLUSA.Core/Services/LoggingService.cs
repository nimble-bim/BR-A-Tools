using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NLog.Config;
using NLog.Targets;
using LogFactory = NLog.LogFactory;

namespace BRPLUSA.Core.Services
{
    public static class LoggingService
    {
        private static Logger Logger { get; set; }
        private static string LogDirectory { get; set; }

        static LoggingService()
        {
            Initialize();
        }

        private static void Initialize()
        {
            SetLogDirectory();
            var config = new LoggingConfiguration();

            var error = CreateErrorLoggingTarget();
            var full = CreateFullLoggingTarget();

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, error);
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, full);

            LogManager.Configuration = config;
            Logger = new LogFactory(config).GetCurrentClassLogger();
        }

        private static void SetLogDirectory()
        {
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = $"{localappdata}/TEMP/BRPLUSA/LOGS/";

            LogDirectory = dir;
        }

        private static FileTarget CreateErrorLoggingTarget()
        {
            var logError = new FileTarget("log_error")
            {
                FileName =  LogDirectory + "${shortdate}/${longdate}_error.log",
                Layout = "${longdate}\t${level}\t${message}\t\t${exception}",
                Header = "*********Enhancement_Log_Errors*************",
                Encoding = Encoding.UTF8,
                ArchiveAboveSize = 100,
                MaxArchiveFiles = 100,
                ArchiveFileName = "archive_${shortdate}",
                ArchiveOldFileOnStartup = true,
                DeleteOldFileOnStartup = false,
                EnableFileDelete = true,
                CreateDirs = true,
                ConcurrentWrites = true,
                NetworkWrites = true,
                KeepFileOpen = true
            };

            return logError;
        }

        private static FileTarget CreateFullLoggingTarget()
        {
            var logFull = new FileTarget("log_full")
            {
                FileName = LogDirectory + "${shortdate}/${longdate}_full.log",
                Layout = "${longdate}\t${level}\t${message}\t\t${exception}",
                Header = "*********Enhancement_Log_Errors*************",
                Encoding = Encoding.UTF8,
                ArchiveAboveSize = 100,
                MaxArchiveFiles = 100,
                ArchiveFileName = "archive_${shortdate}",
                ArchiveOldFileOnStartup = true,
                DeleteOldFileOnStartup = false,
                EnableFileDelete = true,
                CreateDirs = true,
                ConcurrentWrites = true,
                NetworkWrites = true,
                KeepFileOpen = true
            };

            return logFull;
        }

        public static void LogError(string msg, Exception e)
        {
            Logger.Error(e, msg);

            var inner = e.InnerException;

            if(inner != null)
                LogError(inner.Message, inner);
        }

        public static void LogInfo(string msg)
        {
            Logger.Info(msg);
        }

        public static void LogWarning(string msg)
        {
            Logger.Warn(msg);
        }
    }
}
