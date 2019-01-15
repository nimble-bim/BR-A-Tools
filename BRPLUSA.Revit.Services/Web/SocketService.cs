using System;
using BRPLUSA.Revit.Core.Interfaces;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace BRPLUSA.Revit.Services.Web
{
    public class SocketService : ISocketProvider
    {
        private IO.Options Options { get; set; }
        private Socket Socket { get; set; }
        public string RevitId { get; private set; }
        public string ClientUri { get; private set; }
        public string ServerUri { get; private set; }

        public SocketService(string url, bool productionMode)
        {
            Initialize(url, productionMode);
        }

        private void Initialize(string clientUrl, bool inProduction = true)
        {
            var production = "https://cmd-center-api.herokuapp.com/";
            var debug = "http://localhost:4422/";

            RevitId = Guid.NewGuid().ToString();
            ServerUri = inProduction ? production : debug;
            Socket = IO.Socket(ServerUri, Options);
            var revitsocketid = Socket.Io().EngineSocket.Id;
            ClientUri = $"{clientUrl}?revitappid={RevitId}&debug=true";
            Options = new IO.Options()
            {
                IgnoreServerCertificateValidation = true,
                AutoConnect = true,
                ForceNew = true
            };
            
            SetSockets();
        }

        private void SetSockets()
        {
            Socket.On(Socket.EVENT_CONNECT, () =>
            {
                var id = new
                {
                    revit = RevitId,
                    socket = Socket.Io().EngineSocket.Id,
                };

                Socket.Emit("REVIT_CONNECTION_START", JObject.FromObject(id));
            });
        }

        public void AddSocketEvent(string eventName, Action callback)
        {
            Socket.On(eventName, callback);
        }
    }
}
