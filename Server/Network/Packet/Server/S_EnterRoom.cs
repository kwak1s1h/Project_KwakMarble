namespace Network.Packet.Server
{
    public class S_EnterRoom : IPacket
    {
        public Player player;

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
