using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Interfaces;

namespace BRPLUSA.Revit.Services.Updaters
{
    public class ManualModelBackupService
    {
        private ModelBackupService BackupService { get; set; }

        public ManualModelBackupService(ModelBackupService service)
        {
            BackupService = service;
        }

        //public void Register(ISocketProvider service, Document doc)
        //{
        //    //BackupService.RegisterManualBackup(doc);
        //}

        //public void Deregister()
        //{
        //    BackupService.DeregisterManualBackup();
        //}
    }
}
