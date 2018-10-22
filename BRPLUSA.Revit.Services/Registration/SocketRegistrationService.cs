using System.Collections.Generic;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Revit.Entities.Interfaces;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Services.Registration
{
    public static class SocketRegistrationService
    {
        private static readonly List<ISocketConsumptionService> _services = new List<ISocketConsumptionService>();

        public static void AddRegisterableServices(params ISocketConsumptionService[] services)
        {
            foreach (var serv in services)
            {
                AddRegisterableServices(serv);
            }
        }

        public static void AddRegisterableServices(ISocketConsumptionService serv)
        {
            _services.Add(serv);
        }

        public static ISocketService InitializeSocketService(string url = "http://localhost:4422")
        {
            // TODO: upgrade to factory at some point
            return new SocketService();
        }

        public static void RegisterServices(object sender, DocumentOpenedEventArgs args)
        {
            var sock = InitializeSocketService();

            foreach (var serv in _services)
            {
                serv.Register(sock, args.Document);
            }
        }

        public static void DeregisterServices(object sender, DocumentClosedEventArgs args)
        {
            foreach (var serv in _services)
            {
                serv.Deregister();
            }
        }
    }
}
