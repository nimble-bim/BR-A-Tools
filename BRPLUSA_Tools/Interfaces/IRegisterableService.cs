using Autodesk.Revit.DB;

namespace BRPLUSA.Interfaces
{
    public interface IRegisterableService
    {
        void Register(Document doc);
        void Deregister();
    }
}