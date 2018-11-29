using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public interface IProductHandler
    {
        UpdateManager UpdateManager { get; set; }
        FileInstallationService FileReplicationService { get; set; }
    }
}