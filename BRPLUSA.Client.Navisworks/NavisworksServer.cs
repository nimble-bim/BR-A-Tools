using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Automation;

namespace BRPLUSA.Navisworks
{
    public static class NavisworksServer
    {
        private static Application _app;
        private static NavisworksApplication _appAuto;

        private static void StartNavisworksInstance()
        {
            _appAuto = new NavisworksApplication { Visible = true };
            _appAuto.StayOpen();
        }

        private static void CloseNavisworksInstance()
        {
            if(_app != null)
                _appAuto.Dispose();
        }

        public static void InitiateClashDetection(string fileName)
        {
            if (_app == null)
                StartNavisworksInstance();

            _appAuto.OpenFile(fileName);

        }
    }
}
