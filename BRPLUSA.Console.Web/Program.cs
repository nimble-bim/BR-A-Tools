using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            OpenLocalModel();
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

        static void OpenLocalModel()
        {
            var fileName = @"ELEC - 181 Mercer - v2017_PSmith.rvt";
            var file = $@"C:\Users\psmith\Documents\{fileName}";
            var dest = $@"C:\Users\psmith\Desktop\resaved\{fileName}";
            var destFolder = Directory.GetParent(dest).FullName;

            if (!File.Exists(file))
            {
                System.Console.WriteLine("Idiot");
                return;
            }

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            //Process.Start(file);
            File.Copy(file, dest);
        }
    }
}
