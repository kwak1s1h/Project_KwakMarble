namespace Network.Packet.Server
{
    public class S_RoomInfo : IPacket
    {
        public List<Player> players;
        public Player hostInfo;

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

    [Serializable]
    public class Player
    {
        public int id;
    }
}
