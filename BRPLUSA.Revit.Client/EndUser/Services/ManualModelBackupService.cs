using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Interfaces;
using BRPLUSA.Revit.Services.Updates;

namespace BRPLUSA.Revit.Client.EndUser.Services
{
    public class ManualModelBackupService : ISocketConsumer
    {
        private ModelBackupService BackupService { get; set; }

        private void Initialize(Document doc, ISocketProvider service)
        {
            BackupService = new ModelBackupService(doc, service);
        }

        public void Register(ISocketProvider service, Document doc)
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
