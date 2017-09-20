using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;

namespace BRPLUSA_Tools
{
    public class RegistrationService
    {
        private Document _doc;
        private UIControlledApplication _app;
        private readonly List<IRegisterableService> _services;
        public RegistrationService(UIControlledApplication app)
        {
            _services = new List<IRegisterableService>();

            _app = app;
            _app.ControlledApplication.DocumentOpened += RegisterServices;
            _app.ControlledApplication.DocumentClosed += DeregisterServices;
        }

        public void RegisterServices(IEnumerable<IRegisterableService> services)
        {
            foreach (var serv in services)
            {
                RegisterServices(serv);
            }
        }

        public void RegisterServices(IRegisterableService serv)
        {
            _services.Add(serv);
        }

        private void RegisterServices(object sender, DocumentOpenedEventArgs args)
        {
            _doc = args.Document;
            foreach (var serv in _services)
            {
                serv.Register(_doc);
            }
        }

        private void DeregisterServices(object sender, DocumentClosedEventArgs args)
        {
            _app.ControlledApplication.DocumentOpened -= RegisterServices;
            _app.ControlledApplication.DocumentClosed -= DeregisterServices;
            foreach (var serv in _services)
            {
                serv.Deregister();
            }
        }
    }
}
