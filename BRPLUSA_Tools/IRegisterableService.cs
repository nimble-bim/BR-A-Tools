using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BRPLUSA_Tools
{
    public interface IRegisterableService
    {
        void Register(Document doc);
        void Deregister();
    }

    public abstract class RegisterableService : IRegisterableService
    {
        protected static AddInId _appId;
        protected static UpdaterId _updaterId;
        protected static UIControlledApplication _app;
        protected static Document _doc;

        public abstract void Register(Document doc);
        public abstract void Deregister();
    }

    public abstract class RegisterableUpdater : RegisterableService, IUpdater
    {
        public abstract void Execute(UpdaterData data);
        public abstract UpdaterId GetUpdaterId();
        public abstract ChangePriority GetChangePriority();
        public abstract string GetUpdaterName();
        public abstract string GetAdditionalInformation();

        public override void Register(Document doc)
        {
            UpdaterRegistry.RegisterUpdater(this);
        }

        public override void Deregister()
        {
            UpdaterRegistry.UnregisterUpdater(GetUpdaterId());
        }
    }
}