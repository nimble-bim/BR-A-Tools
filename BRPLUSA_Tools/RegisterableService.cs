using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BRPLUSA_Tools
{
    public abstract class RegisterableService : IRegisterableService
    {
        protected static AddInId _appId;
        protected static UpdaterId _updaterId;
        protected static UIControlledApplication _app;
        protected static Document _doc;

        public abstract void Register(Document doc);
        public abstract void Deregister();
    }
}