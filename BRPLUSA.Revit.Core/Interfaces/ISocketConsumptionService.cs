using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Core.Interfaces
{
    public interface ISocketConsumer
    {
        void Register(ISocketProvider service, Document doc);
        void Deregister();
    }
}