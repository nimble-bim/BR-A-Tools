using Autodesk.Revit.DB;

namespace BRPLUSA.Client.Revit.Interfaces
{
    public interface IRegisterableUpdater
    {
        void Register(Document doc);
        void Deregister();
    }
}