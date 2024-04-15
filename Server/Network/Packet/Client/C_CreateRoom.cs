
namespace Network.Packet.Client
{
    public class C_CreateRoom : IPacket
    {
        public ushort Protocol => throw new NotImplementedException();

        public void Read(ArraySegment<byte> segment)
        {

        }

        public ArraySegment<byte> Write()
        {
            return new ArraySegment<byte>();
        }
    }
}
