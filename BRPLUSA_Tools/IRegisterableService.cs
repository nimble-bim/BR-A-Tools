using System;
using Autodesk.Revit.DB;

namespace BRPLUSA_Tools
{
    public interface IRegisterableService
    {
        void Register(Document doc);
        void Deregister();
    }
}