using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BRPLUSA.Revit.Client.Updaters
{
    public class ModelBackupUpdater : IRegisterableUpdater
    {
        private string _modelPath;
        private Document _doc;

        public void Register(Document doc)
        {
            RegisterBIM360Backup(doc);
        }

        public void Deregister()
        {
            _doc.Application.DocumentSynchronizedWithCentral -= RequestModelBackup;
        }

        public void RegisterBIM360Backup(Document doc)
        {
            _modelPath = doc.PathName;
            _doc = doc;
            doc.Application.DocumentSynchronizedWithCentral += RequestModelBackup;
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
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(_modelPath);
            var cult = new CultureInfo("nl-NL");
            Thread.CurrentThread.CurrentCulture = cult;
            var now = DateTime.UtcNow.ToShortDateString() + "_" + DateTime.UtcNow.ToLongTimeString();
            var backupFilePath = $@"{desktop}\_bim360backups\{now}\{fileName}.rvt";
            var backupFolder = Directory.GetParent(backupFilePath).FullName;

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            doc.Application.CopyModel(doc.GetWorksharingCentralModelPath(), backupFolder, true);
        }


        public void BackupModelLocally()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(_modelPath);
            var now = DateTime.UtcNow.ToShortDateString() +"_" + DateTime.UtcNow.ToLongTimeString();
            var backupFilePath = $@"{desktop}\_bim360backups\{fileName}_{now}.rvt";
            var backupFolder = Directory.GetParent(backupFilePath).FullName;

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            //File.Copy(_localModelPath, backupFilePath);
        }
    }
}
