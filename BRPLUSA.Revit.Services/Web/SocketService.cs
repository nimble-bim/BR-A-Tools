using System;
using BRPLUSA.Revit.Entities.Interfaces;
using Quobject.SocketIoClientDotNet.Client;

namespace BRPLUSA.Revit.Services.Web
{
    public class SocketService : ISocketService
    {
        private readonly string url = "http://localhost:4422";
        private IO.Options Options { get; set; }
        private Socket Socket { get; set; }
        public string Id { get; private set; }
        public string Location { get; private set; }

        public SocketService()
        {
            Initialize();
        }

        private void Initialize()
        {
            Id = new Guid().ToString();
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
