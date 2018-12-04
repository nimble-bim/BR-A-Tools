using System;

namespace BRPLUSA.Revit.Core.Interfaces
{
    public interface ISocketProvider
    {
        void AddSocketEvent(string eventName, Action callback);
    }
}