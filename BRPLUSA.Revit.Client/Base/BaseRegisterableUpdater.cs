using Autodesk.Revit.DB;
using BRPLUSA.Revit.Entities.Base;

namespace BRPLUSA.Revit.Client.Base
{
    public abstract class BaseRegisterableUpdater : BaseRegisterableService, IUpdater
    {
        public abstract void Execute(UpdaterData data);
        public abstract UpdaterId GetUpdaterId();
        public abstract ChangePriority GetChangePriority();
        public abstract string GetUpdaterName();
        public abstract string GetAdditionalInformation();

        public override void Register(Document doc)
        {
            _doc = doc;
            UpdaterRegistry.RegisterUpdater(this);
            
        }

        public override void Deregister()
        {
            UpdaterRegistry.UnregisterUpdater(GetUpdaterId());
        }
    }
}