using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packet.Server
{
    public class S_Ready : IPacket
    {
        public int id;
        public bool value;

        public ushort Protocol { get { return (ushort)PacketID.S_SetTurn; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            id = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            value = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            count += sizeof(bool);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(value), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}
