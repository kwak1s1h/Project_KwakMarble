using ServerCore;
using System;
using System.Text;

namespace Server.Packet.Client
{
    public class C_EnterRoom : IPacket
    {
        public ushort Protocol { get { return (ushort)PacketID.C_EnterRoom; } }

        public string roomId;

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);

            ushort nameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            this.roomId = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, nameLen);
            count += nameLen;
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            ushort nameLen = (ushort)Encoding.Unicode.GetBytes(
                this.roomId, 0, this.roomId.Length, segment.Array, segment.Offset + sizeof(ushort) + count);   // UTF16으로 name의 길이 반환
            Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += nameLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}