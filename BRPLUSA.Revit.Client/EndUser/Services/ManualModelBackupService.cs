using System;
using Autodesk.Revit.DB;
using BRPLUSA.Revit.Entities.Base;
using BRPLUSA.Revit.Services.Updates;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Client.EndUser.Services
{
    public class ManualModelBackupService : BaseRegisterableService
    {
        private ModelBackupService BackupService { get; set; }

        public ManualModelBackupService(Document doc, SocketService serv)
        {
            Initialize(doc, serv);
        }

        private void Initialize(Document doc, SocketService serv)
        {
            BackupService = new ModelBackupService(doc, serv);
        }

        public override void Register(Document doc)
        {
            BackupService.RegisterManualBackup(doc);
        }

        public override void Deregister()
        {
            BackupService.DeregisterManualBackup();
        }
    }
}
