using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        static RoomManager _room = new RoomManager();
        public static RoomManager instance { get { return _room; } }

        private Dictionary<string, GameRoom> _rooms = new Dictionary<string, GameRoom>();
        object _lock = new object();

        public GameRoom Create(ClientSession session)
        {
            string id = Guid.NewGuid().ToString();
            lock (_lock)
            {
                GameRoom room = new GameRoom();
                room.Id = id;
                room.Enter(session);
                _rooms.Add(room.Id, room);
                return room;
            }
        }

        public GameRoom Find(string roomId)
        {
            lock (_lock)
            {
                if(!_rooms.TryGetValue(roomId, out GameRoom room))
                {
                    return null;
                }
                return room;
            }
        }
    }
}
