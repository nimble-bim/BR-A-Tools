using System;

namespace BRPLUSA.Revit.Entities.Interfaces
{
    public interface ISocketService
    {
        void AddSocketEvent(string eventName, Action callback);
    }
}