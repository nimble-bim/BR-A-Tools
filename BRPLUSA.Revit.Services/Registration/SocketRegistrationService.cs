using System.Collections.Generic;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Revit.Core.Interfaces;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Services.Registration
{
    public static class SocketRegistrationService
    {
        private static readonly List<ISocketConsumer> _services = new List<ISocketConsumer>();

        public static void AddRegisterableServices(params ISocketConsumer[] services)
        {
            foreach (var serv in services)
            {
                AddRegisterableServices(serv);
            }
        }

        public static void AddRegisterableServices(ISocketConsumer serv)
        {
            _services.Add(serv);
        }

        public static ISocketProvider InitializeSocketService(bool productionMode = true)
        {
            var clientUrl = productionMode 
                ? "https://cmd-center-client.herokuapp.com/"
                : "http://localhost:3000";

            // TODO: upgrade to factory at some point
            return new SocketService(clientUrl);
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
