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
        private Document Document { get; set; }
        private ExternalEvent BackupEvent { get; set; }
        private ISocketProvider SocketService { get; set; }

        public ModelBackupService(Document doc)
        {
            Initialize(doc);
        }

        public ModelBackupService(Document doc, ISocketProvider service)
        {
            Initialize(doc, service);
        }

        private void Initialize(Document doc)
        {
            Document = doc;
            RegisterBackupEventHandler();
        }

        private void Initialize(Document doc, ISocketProvider service)
        {
            SocketService = service;
            Initialize(doc);
        }

        public void RegisterManualBackup(Document doc)
        {
            RegisterSocketService();
        }

        public void DeregisterManualBackup()
        {
            DeregisterAutoBackup();
        }

        public void RegisterAutoBackup()
        {
            var mPath = Document.GetWorksharingCentralModelPath();
            var central = ModelPathUtils.ConvertModelPathToUserVisiblePath(mPath);

            if (IsFromBIM360(central))
                Document.Application.DocumentSynchronizedWithCentral += RequestModelBackup;
        }

        public void DeregisterAutoBackup()
        {
            Document.Application.DocumentSynchronizedWithCentral -= RequestModelBackup;
        }

        private void RegisterSocketService()
        {
            SocketService.AddSocketEvent("BACKUP_REQUESTED", HandleBackupRequest);
        }

        public  void RegisterBackupEventHandler()
        {
            BackupEvent = ExternalEvent.Create(new BackupHandler());
        }

        public  void HandleBackupRequest()
        {
            LoggingService.LogInfo("Received model backup requested");
            BackupEvent.Raise();
            LoggingService.LogInfo("Attempting to backup model");
        }

        private bool IsFromBIM360(string centralPath)
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
            var handler = new BackupHandler();
            handler.BackupModelLocallyUsingRevit(doc);
        }
    }
}
