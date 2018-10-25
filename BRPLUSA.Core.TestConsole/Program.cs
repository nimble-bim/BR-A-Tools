using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Core.Services;

namespace BRPLUSA.Core.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LoggingService.LogInfo("This should get logged as info");
                LoggingService.LogWarning("this should get logged as a warning");
                LoggingService.LogInfo("This should get logged as info");
                LoggingService.LogWarning("this should get logged as a warning");
                LoggingService.LogInfo("This should get logged as info");
                LoggingService.LogWarning("this should get logged as a warning");

                throw new Exception("Fake exception");
            }

            catch (Exception e)
            {
                LoggingService.LogError("This should log an exception and it's inner exception", e);
            }
        }
    }
}
