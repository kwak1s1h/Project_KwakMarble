using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packet.Server
{
    public class S_EnterRoomRes : IPacket
    {
        public bool success;
        public List<int> users = new List<int>();
        public string roomId;

        public ushort Protocol { get { return (ushort)PacketID.S_EnterRoomRes; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.success = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            if (!success) return;
            count += sizeof(bool);
            this.users.Clear();
            ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            for (int i = 0; i < playerLen; i++)
            {
                int id = BitConverter.ToInt32(segment.Array, segment.Offset + count);
                count += sizeof(int);
                users.Add(id);
            }

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
            Array.Copy(BitConverter.GetBytes(success), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);
            Array.Copy(BitConverter.GetBytes((ushort)users.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            foreach (int id in this.users)
            {
                Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + count, sizeof(int));
                count += sizeof(int);
            }
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
