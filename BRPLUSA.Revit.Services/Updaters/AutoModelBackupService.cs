using Autodesk.Revit.DB;
using BRPLUSA.Revit.Core.Base;

namespace BRPLUSA.Revit.Services.Updaters
{
    public class AutoModelBackupService : BaseRegisterableService
    {
        private ModelBackupService BackupService { get; set; }
        private Document Document { get; set; }

        public AutoModelBackupService(ModelBackupService service)
        {
            BackupService = service;
        }

        public override void Register(Document doc)
        {
            Document = doc;
            RegisterAutoBackup();
        }

        public override void Deregister()
        {
            DeregisterAutoBackup();
        }

        public void RegisterAutoBackup()
        {
            var mPath = Document.GetWorksharingCentralModelPath();
            var central = ModelPathUtils.ConvertModelPathToUserVisiblePath(mPath);

            if (BackupService.IsFromBIM360(central))
                Document.Application.DocumentSynchronizedWithCentral += BackupService.RequestModelBackup;
        }

        public void DeregisterAutoBackup()
        {
            Document.Application.DocumentSynchronizedWithCentral -= BackupService.RequestModelBackup;
        }
    }
}
