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
            //BackupModelLocallyUsingRevit(app.ActiveUIDocument.Document);
            BackupModelLocally(app.ActiveUIDocument.Document);
            ShowTaskCompleted();
        }

        public string GetName()
        {
            return "Model Backup Handling Service";
        }

        public void ShowTaskCompleted()
        {
            TaskDialog.Show("Done", "Completed task!");
        }

        public void BackupModelLocallyUsingRevit(Document doc)
        {
            var modelPath = doc.PathName;

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
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

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var fileName = Path.GetFileNameWithoutExtension(modelPath);
            var cult = new CultureInfo("nl-NL");
            Thread.CurrentThread.CurrentCulture = cult;
            var now = DateTime.UtcNow.ToShortDateString() + "_" + DateTime.UtcNow.ToLongTimeString();
            now = now.Replace(":", String.Empty);
            var backupFilePath = $@"{desktop}\_bim360backups\{now}\{fileName}.rvt";
            var backupFolder = Directory.GetParent(backupFilePath).FullName;

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);


            File.Copy(modelPath, backupFilePath);
        }
    }
}
