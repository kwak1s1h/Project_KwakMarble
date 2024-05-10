using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IPacket
    {
        ushort Protocol { get; }
        void Read(ArraySegment<byte> segment);
        ArraySegment<byte> Write();
    }
}