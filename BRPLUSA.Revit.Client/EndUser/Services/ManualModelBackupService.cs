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

        public ManualModelBackupService(SocketService serv)
        {
            Initialize(serv);
        }

        private void Initialize(SocketService serv)
        {
            BackupService = new ModelBackupService(serv);
        }

        public override void Register(Document doc)
        {
            BackupService.Register(doc);
        }

        public override void Deregister()
        {
            BackupService.Deregister();
        }
    }
}
