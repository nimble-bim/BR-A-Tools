using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BRPLUSA.Revit.Client.Interfaces;

namespace BRPLUSA.Revit.Client.Base
{
    public abstract class BaseRegisterableService : IRegisterableUpdater
    {
        protected static AddInId _appId;
        protected static UpdaterId _updaterId;
        protected static UIControlledApplication _app;
        protected static Document _doc;

        public abstract void Register(Document doc);
        public abstract void Deregister();
    }
}