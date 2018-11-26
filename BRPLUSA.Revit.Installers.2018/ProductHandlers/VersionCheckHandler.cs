using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BRPLUSA.Revit.Installers._2018.Services;
using Squirrel;

namespace BRPLUSA.Revit.Installers._2018.ProductHandlers
{
    public class ProductVersionHandler : BaseProductHandler
    {
        private bool? _isOutdated = null;
        private int? _localVersion = null;
        private int? _serverVersion = null;

        public ProductVersionHandler(UpdateManager mgr, FileReplicationService frp) 
            : base(mgr, frp) { }

        public bool ShouldUpdate
        {
            get { return _isOutdated ?? true; }
        }

        public ReleaseEntry GetReleaseInfo()
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateInfo> GetVersionInformationFromServer()
        {
            try
            {
                var info = await UpdateManager.CheckForUpdate();

                _isOutdated = _localVersion < _serverVersion;

                return info;
            }

            catch (Exception e)
            {
                throw new Exception("Failed to retrieve information", e);
            }
        }
    }
}