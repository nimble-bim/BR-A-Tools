using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Core.Interfaces;
using BRPLUSA.Revit.Services.Handlers;
using BRPLUSA.Revit.Services.Web;

namespace BRPLUSA.Revit.Services.Updates
{
    public class ModelBackupService
    {
        private static Document Document { get; set; }
        private static ExternalEvent BackupEvent { get; set; }
        private static ISocketService SocketService { get; set; }

        public ModelBackupService(Document doc)
        {
            Initialize(doc);
        }

        public ModelBackupService(Document doc, ISocketService service)
        {
            Initialize(doc, service);
        }

        private void Initialize(Document doc)
        {
            Document = doc;
            RegisterBackupEventHandler();
        }

        private void Initialize(Document doc, ISocketService service)
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

        public static void RegisterBackupEventHandler()
        {
            BackupEvent = ExternalEvent.Create(new BackupHandler());
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
            BackupModelLocallyUsingRevit(args.Document);
        }

        public void BackupModelLocallyUsingRevit(Document doc)
        {
            var handler = new BackupHandler();
            handler.BackupModelLocallyUsingRevit(doc);
        }
    }
}
