using System;
using BRPLUSA.Revit.Core.Interfaces;
using Quobject.SocketIoClientDotNet.Client;

namespace BRPLUSA.Revit.Services.Web
{
    public class SocketService : ISocketProvider
    {
        private IO.Options Options { get; set; }
        private Socket Socket { get; set; }
        public string Id { get; private set; }
        public string Location { get; private set; }

        public SocketService(string url)
        {
            Initialize(url);
        }

        private void Initialize(string url)
        {
            Id = Guid.NewGuid().ToString();
            Options = new IO.Options()
            {
                IgnoreServerCertificateValidation = true,
                AutoConnect = true,
                ForceNew = true
            };

            Socket = IO.Socket(url, Options);
            SetSockets();
        }

        private void SetSockets()
        {
            Socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected to a new client");
                Socket.Emit("ROOM_CREATE", Id);
            });
        }

        public void AddSocketEvent(string eventName, Action callback)
        {
            Socket.On(eventName, callback);
        }
    }
}
