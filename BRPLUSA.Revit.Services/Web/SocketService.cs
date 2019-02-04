using System;
using BRPLUSA.Core.Services;
using BRPLUSA.Revit.Core.Interfaces;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace BRPLUSA.Revit.Services.Web
{
    public class SocketService : ISocketProvider
    {
        private WebSocket Socket { get; set; }
        public string RevitId { get; private set; }
        public string ClientUri { get; private set; }
        public string ServerUri { get; private set; }

        public SocketService(string url, bool productionMode)
        {
            Initialize(url, productionMode);
        }

        private void Initialize(string clientUrl, bool inProduction = true)
        {
            var production = "ws://cmd-center-api.herokuapp.com/";
            var debug = "ws://localhost:4422/";

            RevitId = Guid.NewGuid().ToString();
            ServerUri = inProduction ? production : debug;
            ClientUri = inProduction
                ? $"{clientUrl}?revitappid={RevitId}"
                : $"{clientUrl}?revitappid={RevitId}&debug=true";

            Socket = new WebSocket(ServerUri);

            SetSockets();

            Socket.Connect();
        }

        private void SetSockets()
        {
            Socket.OnMessage += (sender, e) =>
            {
                Console.WriteLine($"Server says: {e.Data}");
                LoggingService.LogInfo($"Server says: {e.Data}");

                if (e.Data == "BACKUP_REQUESTED")
                    LoggingService.LogInfo("Backup was requested successfully");
            };

            Socket.OnError += (sender, e) => { Console.WriteLine($"There was an error: {e.Message}"); };

            Socket.OnClose += (sender, e) => { Console.WriteLine($"The socket is closing: {e.Reason}"); };

            Socket.OnOpen += (sender, e) =>
            {
                Console.WriteLine($"The socket is opening...");

                var id = new
                {
                    revit = RevitId,
                };

                LoggingService.LogInfo($"Connected to socket server for Revit document: {id.revit}");

                //Socket.Send("REVIT_CONNECTION_START");
            };

            

            //Socket.On(Socket.EVENT_CONNECT, () =>
            //{
            //    var id = new
            //    {
            //        revit = RevitId,
            //        socket = Socket.Io().EngineSocket.Id,
            //    };

            //    LoggingService.LogInfo($"Connected to socket server for Revit document: {id.revit}");

            //    Socket.Emit("REVIT_CONNECTION_START", JObject.FromObject(id));
            //});

            //Socket.On(Socket.EVENT_CONNECT_ERROR, (err) =>
            //{
            //    LoggingService.LogError($"There was an error connecting back to the server: {err}", null);
            //});

            //Socket.On(Socket.EVENT_DISCONNECT, (info) =>
            //{
            //    LoggingService.LogInfo($"Disconnected from socket server");
            //    LoggingService.LogInfo($"More information: ${info}");
            //});

            //Socket.On(Socket.EVENT_RECONNECTING, (info) =>
            //{
            //    LoggingService.LogInfo($"Trying to reconnect to the socket server");
            //    LoggingService.LogInfo($"More information: ${info}");
            //});
        }

        public void AddSocketEvent(string eventName, Action callback)
        {
            LoggingService.LogInfo($"Attempting to add socket event called: {eventName}");
            //Socket.On(eventName, callback);
            //Socket.OnMessage += (sender, e) => callback();
            
            LoggingService.LogInfo($"Added socket event called: {eventName}");
        }

        public void Dispose()
        {
            Socket.Close(CloseStatusCode.Normal);
            Socket = null;
        }
    }
}
