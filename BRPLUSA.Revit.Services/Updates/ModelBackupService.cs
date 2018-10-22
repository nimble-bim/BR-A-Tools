using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Services.Handlers;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Services.Updates
{
    public class ModelBackupService
    {
        private string _modelPath;
        private Document Document { get; set; }
        private static ExternalEvent BackupEvent { get; set; }
        private SocketService SocketService { get; set; }

        public ModelBackupService()
        {

        }

        public ModelBackupService(SocketService service)
        {
            SocketService = service;
        }

        private void Initialize(Document doc)
        {
            _modelPath = Document.PathName;
            Document = doc;
        }
        public void Register(Document doc)
        {
            Initialize(doc);
            RegisterBackupEventHandler();
            RegisterAutoBackup();
            RegisterSocketService();
        }

        public void Deregister()
        {
            DeregisterAutoBackup();
        }

        private void RegisterSocketService()
        {
            SocketService.AddSocketEvent("BACKUP_REQUESTED", HandleBackupRequest);
        }

        public static void RegisterBackupEventHandler()
        {
            BackupEvent = ExternalEvent.Create(new BackupHandler());
        }

        public void RegisterAutoBackup()
        {
            Document.Application.DocumentSynchronizedWithCentral += RequestModelBackup;
        }

        public void DeregisterAutoBackup()
        {
            Document.Application.DocumentSynchronizedWithCentral -= RequestModelBackup;
        }

        public static void HandleBackupRequest()
        {
            BackupEvent.Raise();
        }

        private bool IsFromBIM360(string centralPath)
        {
            const string bim360Server = "BIM 360://";
            const string A360server = "A360://";

            return centralPath.StartsWith(bim360Server) || centralPath.StartsWith(A360server);
        }

        public void RequestModelBackup(object sender, DocumentSynchronizedWithCentralEventArgs args)
        {
            var mPath = args.Document.GetWorksharingCentralModelPath();
            var central = ModelPathUtils.ConvertModelPathToUserVisiblePath(mPath);

            if (!IsFromBIM360(central))
                return;

            BackupModelLocallyUsingRevit(args.Document);
        }

        public void BackupModelLocallyUsingRevit(Document doc)
        {
            var handler = new BackupHandler();
            handler.BackupModelLocallyUsingRevit(doc);
        }
    }
}
