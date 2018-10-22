using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Entities.Interfaces
{
    public interface IRegisterableUpdater
    {
        void Register(Document doc);
        void Deregister();
    }
}