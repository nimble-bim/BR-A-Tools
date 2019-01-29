using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Base;

namespace BRPLUSA.Revit.Services.Updaters
{
    public class AutoModelBackupService : BaseRegisterableService
    {
        private ModelBackupService BackupService { get; set; }

        private void Initialize(Document doc)
        {
            BackupService = new ModelBackupService(doc);
        }

        public override void Register(Document doc)
        {
            Initialize(doc);
            BackupService.RegisterAutoBackup();
        }

        public override void Deregister()
        {
            BackupService.DeregisterAutoBackup();
        }
    }
}
