using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BRPLUSA.Revit.Services.Handlers
{
    //TODO: sample Handler for POC - create real one later to take care of tasks
    public class BackupHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            TaskDialog.Show("Done", "Completed task!");
        }

        public string GetName()
        {
            return "Model Backup Handling Service";
        }

        public void BackupModelLocallyUsingRevit(Document doc)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var modelPath = doc.PathName;
            var fileName = Path.GetFileNameWithoutExtension(modelPath);
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

        public void BackupModelLocally(Document doc)
        {
            var modelPath = doc.PathName;

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(modelPath);
            var now = DateTime.UtcNow.ToShortDateString() + "_" + DateTime.UtcNow.ToLongTimeString();
            var backupFilePath = $@"{desktop}\_bim360backups\{fileName}_{now}.rvt";
            var backupFolder = Directory.GetParent(backupFilePath).FullName;

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);


            //File.Copy(_localModelPath, backupFilePath);
        }
    }
}
