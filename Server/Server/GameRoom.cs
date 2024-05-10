using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Packet.Client;
using Server.Packet.Server;
using Server.Session;
using ServerCore;

namespace Server
{
    public class GameRoom
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        object _lock = new object();

        private string _id;
        public string Id
        {
            get => _id; set => _id = value;
        }

        public void Enter(ClientSession session)
        {
            lock(_lock)
            {   // 신규 유저 추가
                S_EnterRoom enter = new S_EnterRoom { id = session.SessionId };
                BroadCast(enter.Write());

                _sessions.Add(session);
                Console.WriteLine($"세션 추가 : {session.SessionId}");

                session.Room = this;
                
                // 신규 유저 접속시, 기존 유저 목록 전송
                //S_PlayerList players = new S_PlayerList();
                //foreach (ClientSession s in _sessions)
                //{
                //    players.players.Add(new S_PlayerList.Player() {
                //        isSelf = (s == session),
                //        playerId = s.SessionId,
                //    });
                //}
                //session.Send(players.Write());

                // 신규 유저 접속 전체 공지
                //S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
                //enter.playerId = session.SessionId;
                //BroadCast(enter.Write());
            }

        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                // 플레이어 제거하고
                _sessions.Remove(session);

                // 모두에게 알린다
                //S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
                //leave.playerId = session.SessionId;
                //BroadCast(leave.Write());
            }
        }

        public void BroadCast(ArraySegment<byte> segment) 
        {
            ArraySegment<byte> packet = segment;

            lock (_lock) // 
            {
                foreach(ClientSession s in _sessions)
                {
                    s.Send(segment);    // 리스트에 들어있는 모든 클라에 전송
                }
            }
        }

        public List<int> GetAllUserInfo()
        {
            return _sessions.Select(s => s.SessionId).ToList();
        }

        private int _turnIdx;
        public void StartGame()
        {
            _turnIdx = 0;
            SetTurn();
        }

        private void SetTurn()
        {
            int id = _sessions[_turnIdx].SessionId;

            S_SetTurn turn = new S_SetTurn { id = id };
            BroadCast(turn.Write());
        }

        public void DrawDice(ClientSession session)
        {
            Random rand = new Random();
            int[] rands = new int[2] { rand.Next(1, 7), rand.Next(1, 7) };

            //BroadCast()

            _turnIdx++;
            if(_turnIdx >= _sessions.Count)
            {
                _turnIdx = 0;
            }
        }
    }
}
