using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;

namespace BRPLUSA.Revit.Services.Export
{
    public class NavisworksExporter : INavisworksExporter
    {
        public Guid GetServerId()
        {
            throw new NotImplementedException();
        }

        public ExternalServiceId GetServiceId()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public string GetVendorId()
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public void Export(Document document, string folder, string name, NavisworksExportOptions options)
        {
            throw new NotImplementedException();
        }

        public bool ValidateExportOptions(Document document, string folder, string name, NavisworksExportOptions options,
            out string exceptionMessage)
        {
            throw new NotImplementedException();
        }
    }
}
