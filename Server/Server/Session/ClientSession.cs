using Network;
using Network.Packet.Server;
using System.Net;

namespace Server.Session
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom? Room { get; set; }

        public Player PlayerInfo => new Player { id = SessionId };

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected bytes: {endPoint}");
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisConnected bytes: {endPoint}");
            SessionManager.instance.Remove(this);
            if (Room != null)
            {
                Room.Leave(this);
                Room = null;
            }
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)             // 샌드 이벤트 발생시 동작
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
