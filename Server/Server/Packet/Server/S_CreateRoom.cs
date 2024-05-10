using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packet.Server
{
    public class S_CreateRoom : IPacket
    {
        public string roomCode;

        public ushort Protocol { get { return (ushort)PacketID.S_CreateRoom; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            ushort codeLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            this.roomCode = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, codeLen);
            count += codeLen;
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            ushort codeLen = (ushort)Encoding.Unicode.GetBytes(
                this.roomCode, 0, this.roomCode.Length, segment.Array, segment.Offset + sizeof(ushort) + count);   // UTF16으로 name의 길이 반환
            Array.Copy(BitConverter.GetBytes(codeLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += codeLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}
