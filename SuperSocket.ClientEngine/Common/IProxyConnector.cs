using System;
using System.Net;

namespace SuperSocket.ClientEngine.Common
{
    public interface IProxyConnector
    {
        void Connect(EndPoint remoteEndPoint);

        event EventHandler<ProxyEventArgs> Completed;
    }
}
