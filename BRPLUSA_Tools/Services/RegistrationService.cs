using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.Updaters;
using BRPLUSA.Revit.Interfaces;

namespace BRPLUSA.Revit.Services
{
    public class RegistrationService
    {
        private Document _doc;
        private readonly UIControlledApplication _app;
        private readonly List<IRegisterableService> _services;
        public RegistrationService(UIControlledApplication app)
        {
            _app = app;
            _services = new List<IRegisterableService>();

            Initialize();
        }

        private void Initialize()
        {
            RegisterServices(new SpatialPropertyUpdater(_app));

            _app.ControlledApplication.DocumentOpened += RegisterServices;
            _app.ControlledApplication.DocumentClosed += DeregisterServices;
        }

        private void RegisterServices(params IRegisterableService[] services)
        {
            foreach (var serv in services)
            {
                RegisterServices(serv);
            }
        }

        private void RegisterServices(IRegisterableService serv)
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
