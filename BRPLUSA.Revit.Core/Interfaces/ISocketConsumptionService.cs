using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Entities.Interfaces
{
    public interface ISocketConsumptionService
    {
        void Register(ISocketService service, Document doc);
        void Deregister();
    }
}