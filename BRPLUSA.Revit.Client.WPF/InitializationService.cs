using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Revit.Client.WPF.Viewers;
using SimpleInjector;

namespace BRPLUSA.Revit.Client.WPF
{
    public class InitializationService
    {
        public static IRevitClient InitializeUIServices()
        {
            var container = new Container();

            container.Register<IRevitClient, BardWpfClient>();

            return container.GetInstance<BardWpfClient>();
        }
    }
}
