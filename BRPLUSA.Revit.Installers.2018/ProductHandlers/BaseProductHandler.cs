using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public abstract class BaseProductHandler : IProductHandler
    {
        public UpdateManager UpdateManager { get; set; }
        public FileReplicationService FileReplicationService { get; set; }

        protected BaseProductHandler(UpdateManager mgr, FileReplicationService frp)
        {
            UpdateManager = mgr;
            FileReplicationService = frp;
        }
    }
}