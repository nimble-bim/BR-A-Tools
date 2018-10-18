using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Interfaces
{
    public interface IRegisterableUpdater
    {
        void Register(Document doc);
        void Deregister();
    }
}