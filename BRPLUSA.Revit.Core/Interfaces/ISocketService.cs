using System;

namespace BRPLUSA.Revit.Core.Interfaces
{
    public interface ISocketService
    {
        void AddSocketEvent(string eventName, Action callback);
    }
}