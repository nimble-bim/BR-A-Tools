using System;
using Autodesk.Revit.DB;
using BRPLUSA.Revit.Entities.Base;
using BRPLUSA.Revit.Services.Updates;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Client.EndUser.Services
{
    public class ManualModelBackupServices : BaseRegisterableService
    {
        private ModelBackupService BackupService { get; set; }

        public ManualModelBackupServices(SocketService serv)
        {
            Initialize(serv);
        }

        private void Initialize(SocketService serv)
        {
            BackupService = new ModelBackupService(serv);

        }

        public override void Register(Document doc)
        {
            throw new NotImplementedException();
        }

        public override void Deregister()
        {
            throw new NotImplementedException();
        }
    }
}
