using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Interfaces;
using BRPLUSA.Revit.Services.Updates;

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
