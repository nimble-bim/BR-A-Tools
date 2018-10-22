using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Core.Interfaces
{
    public interface IRegisterableUpdater
    {
        void Register(Document doc);
        void Deregister();
    }
}