using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Interfaces
{
    public interface IRegisterableService
    {
        void Register(Document doc);
        void Deregister();
    }
}