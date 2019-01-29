using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Revit.Client.WPF.Viewers;
using BRPLUSA.Revit.Services.Updaters;
using SimpleInjector;

namespace BRPLUSA.Revit.Client.WPF
{
    public class InitializationService
    {
        public static Container InitializeUIServices()
        {
            var container = new Container();

            container.Register<IRevitClient, BardWpfClient>();
            container.Register<AutoModelBackupService>();
            container.Register<ManualModelBackupService>();

            return container;
        }
    }
}
