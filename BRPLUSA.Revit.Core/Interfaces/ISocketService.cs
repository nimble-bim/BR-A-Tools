using System;

namespace BRPLUSA.Revit.Core.Interfaces
{
    public interface ISocketProvider : IDisposable
    {
        void AddSocketEvent(string eventName, Action callback);
        string ClientUri { get; }
    }
}