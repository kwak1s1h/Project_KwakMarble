using Network.Packet.Server;
using Server.Session;

namespace Server
{
    public class GameRoom
    {
        private List<ClientSession> _sessions = new List<ClientSession>();
        private ClientSession _hostSession;

        private object _lock = new object();

        public GameRoom(ClientSession hostSession)
        {
            _sessions = new List<ClientSession> { hostSession };
            _hostSession = hostSession;
            hostSession.Room = this;
            hostSession.Send(CreateRoomInfoPacket().Write());
        }

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {   // 신규 유저 추가
                _sessions.Add(session);
                session.Room = this;

                // 신규 유저 접속시, 룸정보 전송
                S_RoomInfo roomInfo = CreateRoomInfoPacket();
                session.Send(roomInfo.Write());

                // 신규 유저 접속 전체 공지
                S_EnterRoom enterInfo = new S_EnterRoom();
                enterInfo.player = new Player { id = session.SessionId };
                BroadCast(enterInfo.Write());
            }

        }

        private S_RoomInfo CreateRoomInfoPacket()
        {
            S_RoomInfo roomInfo = new S_RoomInfo();
            roomInfo.hostInfo = _hostSession.PlayerInfo;
            foreach (ClientSession s in _sessions)
            {
                roomInfo.players.Add(s.PlayerInfo);
            }

            return roomInfo;
        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                // 플레이어 제거하고
                _sessions.Remove(session);

                // 모두에게 알린다
                S_LeaveRoom leaveInfo = new S_LeaveRoom();
                leaveInfo.playerId = session.SessionId;
                session.Room = null;
                BroadCast(leaveInfo.Write());
            }
        }

        public void BroadCast(ArraySegment<byte> segment)
        {
            ArraySegment<byte> packet = segment;

            lock (_lock) // 
            {
                foreach (ClientSession session in _sessions)
                {
                    session.Send(segment);    // 리스트에 들어있는 모든 클라에 전송
                }
            }
        }

    }
}
