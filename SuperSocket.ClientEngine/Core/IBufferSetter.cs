using System;

namespace SuperSocket.ClientEngine.Core
{
    public interface IBufferSetter
    {
        void SetBuffer(ArraySegment<byte> bufferSegment);
    }
}
