using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Revit.Interfaces;
using System;
using System.Collections.Generic;
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
            var mPath = doc.GetWorksharingCentralModelPath();
            var central = ModelPathUtils.ConvertModelPathToUserVisiblePath(mPath);

            if (IsFromBIM360(central))
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
            var info = new ThreadStart(BackupModelLocally);
            var backupThread = new Thread(info);
            //backupThread.IsBackground = true;
            backupThread.Start();
        }

        public void BackupModelLocally()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(_modelPath);
            var now = DateTime.UtcNow.ToShortDateString() + DateTime.UtcNow.ToLongTimeString();
            var backupPath = $@"{desktop}\_bim360backups\{fileName}_{now}.rvt";
        }
    }
}
