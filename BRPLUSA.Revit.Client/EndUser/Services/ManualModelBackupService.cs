using System;
using Autodesk.Revit.DB;
using BRPLUSA.Revit.Entities.Base;
using BRPLUSA.Revit.Entities.Interfaces;
using BRPLUSA.Revit.Services.Updates;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Client.EndUser.Services
{
    public class ManualModelBackupService : ISocketConsumptionService
    {
        private ModelBackupService BackupService { get; set; }

        private void Initialize(Document doc, ISocketService service)
        {
            BackupService = new ModelBackupService(doc, service);
        }

        public void Register(ISocketService service, Document doc)
        {
            Initialize(doc, service);
            BackupService.RegisterManualBackup(doc);
        }

        public void Deregister()
        {
            BackupService.DeregisterManualBackup();
        }
    }
}
