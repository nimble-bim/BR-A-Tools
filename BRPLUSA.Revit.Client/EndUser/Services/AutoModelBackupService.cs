using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using BRPLUSA.Revit.Entities.Base;
using BRPLUSA.Revit.Entities.Interfaces;
using BRPLUSA.Revit.Services.Updates;

namespace BRPLUSA.Revit.Client.EndUser.Services
{
    public class AutoModelBackupService : BaseRegisterableService
    {
        private ModelBackupService BackupService { get; set; }

        public AutoModelBackupService(Document doc)
        {
            BackupService = new ModelBackupService(doc);
        }

        public override void Register(Document doc)
        {
            BackupService.RegisterAutoBackup();
        }

        public override void Deregister()
        {
            BackupService.DeregisterAutoBackup();
        }
    }
}
