using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Domain;
using BRPLUSA.Domain.Entities;

namespace BRPLUSA.Console.Web
{
    public class Program
    {

        static void Main(string[] args)
        {
            RunHerokuLocal();
            //SendData();
            System.Console.Read();
        }

        static async Task SendData()
        {
            var state = new UserOpenedModelEvent("TestMODEL");
            var response = await WorksharingMonitorService.PostModelOpenedEvent(state);
            System.Console.WriteLine(response);
            System.Console.Read();
        }

        static void RunHerokuLocal()
        {
            var cmd = new Process()
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    //WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            cmd.Start();

            cmd.StandardInput.WriteLine("heroku local web");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            System.Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}
