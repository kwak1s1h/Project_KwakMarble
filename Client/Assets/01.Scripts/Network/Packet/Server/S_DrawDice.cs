using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packet.Server
{
    public class S_DrawDice : IPacket
    {
        public int id;
        public int dest;
        public List<int> diceValue = new List<int>();

        public ushort Protocol { get { return (ushort)PacketID.S_DrawDice; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;
            count += sizeof(ushort);
            count += sizeof(ushort);
            this.id = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            this.dest = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
            this.diceValue.Clear();
            ushort playerLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            for (int i = 0; i < playerLen; i++)
            {
                int id = BitConverter.ToInt32(segment.Array, segment.Offset + count);
                count += sizeof(int);
                diceValue.Add(id);
            }
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes(dest), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);
            Array.Copy(BitConverter.GetBytes((ushort)diceValue.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            foreach (int id in this.diceValue)
            {
                Array.Copy(BitConverter.GetBytes(id), 0, segment.Array, segment.Offset + count, sizeof(int));
                count += sizeof(int);
            }

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}
