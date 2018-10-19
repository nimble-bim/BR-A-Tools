using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Entities.Interfaces;
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

        public ModelBackupService(SocketService service)
        {
            SocketService = service;
        }

        public void Register(Document doc)
        {
            Document = doc;
            RegisterBackupEventHandler();
            RegisterBIM360Backup();
            RegisterSocketService();
        }

        private void RegisterSocketService()
        {
            SocketService.AddSocketEvent("BACKUP_REQUESTED", HandleBackupRequest);
        }

        public static void RegisterBackupEventHandler()
        {
            BackupEvent = ExternalEvent.Create(new TestingHandler());
        }

        public void RegisterBIM360Backup()
        {
            _modelPath = Document.PathName;
            Document.Application.DocumentSynchronizedWithCentral += RequestModelBackup;
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
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(_modelPath);
            var cult = new CultureInfo("nl-NL");
            Thread.CurrentThread.CurrentCulture = cult;
            var now = DateTime.UtcNow.ToShortDateString() + "_" + DateTime.UtcNow.ToLongTimeString();
            now = now.Replace(":", String.Empty);
            var backupFilePath = $@"{desktop}\_bim360backups\{now}\{fileName}.rvt";
            var backupFolder = Directory.GetParent(backupFilePath).FullName;

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            doc.Application.CopyModel(doc.GetWorksharingCentralModelPath(), backupFilePath, true);
        }

        public void BackupModelLocally()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(_modelPath);
            var now = DateTime.UtcNow.ToShortDateString() + "_" + DateTime.UtcNow.ToLongTimeString();
            var backupFilePath = $@"{desktop}\_bim360backups\{fileName}_{now}.rvt";
            var backupFolder = Directory.GetParent(backupFilePath).FullName;

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            //File.Copy(_localModelPath, backupFilePath);
        }
    }
}
