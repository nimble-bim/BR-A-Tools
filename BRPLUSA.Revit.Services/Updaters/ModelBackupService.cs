using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Core.Interfaces;
using BRPLUSA.Revit.Services.Handlers;

namespace BRPLUSA.Revit.Services.Updaters
{
    public class ModelBackupService
    {
        private ExternalEvent BackupEvent { get; set; }
        private BackupHandler Handler { get; }

        public ModelBackupService(BackupHandler handler)
        {
            Handler = handler;
            RegisterBackupEventHandler();
        }

        public void RegisterBackupEventHandler()
        {
            BackupEvent = ExternalEvent.Create(Handler);
        }

        public void HandleBackupRequest()
        {
            LoggingService.LogInfo("Received model backup requested");
            BackupEvent.Raise();
            LoggingService.LogInfo("Attempting to backup model");
        }

        public bool IsFromBIM360(string centralPath)
        {
            const string bim360Server = "BIM 360://";
            const string A360server = "A360://";

            return centralPath.StartsWith(bim360Server) || centralPath.StartsWith(A360server);
        }

        public void RequestModelBackup(object sender, DocumentSynchronizedWithCentralEventArgs args)
        {
            BackupModelLocallyUsingRevit(args.Document);
        }

        public void BackupModelLocallyUsingRevit(Document doc)
        {
            Handler.BackupModelLocallyUsingRevit(doc);
        }
    }
}
