using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Packet.Client
{
    public class C_LeaveRoom : IPacket
    {
        public ushort Protocol => throw new NotImplementedException();

        public void Read(ArraySegment<byte> segment)
        {
            throw new NotImplementedException();
        }

        public ArraySegment<byte> Write()
        {
            throw new NotImplementedException();
        }
    }
}
