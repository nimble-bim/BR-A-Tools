using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Providers;

namespace BRPLUSA.Revit.Installers._2018.Services
{
    public class InstallStatusService
    {
        public bool IsRevit2018Installed
        {
            get
            {
                return RevitAddinLocationProvider.IsRevitVersionInstalled(RevitVersion.V2018);
            }
        }
    }
}
