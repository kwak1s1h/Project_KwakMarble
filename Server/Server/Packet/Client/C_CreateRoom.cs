using ServerCore;
using System;

namespace Server.Packet.Client
{
    public class C_CreateRoom : IPacket
    {
        public ushort Protocol { get { return (ushort)PacketID.C_CreateRoom; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}