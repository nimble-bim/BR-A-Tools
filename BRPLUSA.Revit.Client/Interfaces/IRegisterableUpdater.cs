using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Client.Interfaces
{
    public interface IRegisterableUpdater
    {
        void Register(Document doc);
        void Deregister();
    }
}