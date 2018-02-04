using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Domain;
using BRPLUSA.Domain.Entities;

namespace BRPLUSA.Console.Web
{
    public class Program
    {

        static void 
            Main(string[] args)
        {
            SendData();
            System.Console.Read();
        }

        static async Task SendData()
        {
            var state = new UserOpenedModelEvent("TestMODEL");
            var response = await WorksharingMonitorService.PostModelOpenedEvent(state);
            System.Console.WriteLine(response);
            System.Console.Read();
        }
    }
}
