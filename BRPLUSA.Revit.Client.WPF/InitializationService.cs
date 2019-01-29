using System;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Client.WPF.Commands;
using BRPLUSA.Revit.Client.WPF.Pages;
using BRPLUSA.Revit.Client.WPF.Viewers;
using BRPLUSA.Revit.Client.WPF.ViewModels;
using BRPLUSA.Revit.Services.Handlers;
using BRPLUSA.Revit.Services.Updaters;
using SimpleInjector;

namespace BRPLUSA.Revit.Client.WPF
{
    public class InitializationService
    {
        public static Container InitializeUIServices()
        {
            try
            {
                var container = new Container();

                container.Register<IRevitClient, BardWpfClient>();
                container.Register<AutoModelBackupService>();
                container.Register<ManualModelBackupService>();
                container.Register<ModelBackupService>();
                container.Register<BackupHandler>();

                container.Register<BackupModelCommand>();
                container.Register<BackupPageViewModel>();
                container.Register<BackupPageContent>();

                container.Verify();

                return container;
            }
            catch(Exception e)
            {
                LoggingService.LogError("Couldn't initialize UI Services", e);
                throw e;
            }
        }
    }
}
