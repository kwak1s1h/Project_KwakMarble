using Server.Packet.Client;
using Server.Packet.Server;
using Server.Session;
using ServerCore;
using System;

namespace Server
{
    class PacketHandler 
    {

        public static void C_CreateRoomHandler(PacketSession session, IPacket packet)
        {
            C_CreateRoom createPacket = packet as C_CreateRoom;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;

            clientSession.Room = RoomManager.instance.Create(clientSession);
            S_CreateRoom create = new S_CreateRoom();
            create.roomCode = clientSession.Room.Id;
            clientSession.Send(create.Write());
        }

        public static void C_SetNicknameHandler(PacketSession session, IPacket packet)
        {
            C_SetNickname namePacket = packet as C_SetNickname;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
            clientSession.Nickname = namePacket.value;
        }

        public static void C_EnterRoomHandler(PacketSession session, IPacket packet)
        {
            C_EnterRoom roomPacket = packet as C_EnterRoom;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
            GameRoom room = RoomManager.instance.Find(roomPacket.roomId);
            S_EnterRoomRes res = new S_EnterRoomRes();
            if (room == null)
            {
                res.success = false;
                clientSession.Room = null;
                clientSession.Send(res.Write());
                return;
            }

            res.success = true;
            room.Enter(clientSession);
            res.users = room.GetAllUserInfo();
            res.roomId = room.Id;
            clientSession.Send(res.Write());
        }

        public static void C_ReadyHandler(PacketSession session, IPacket packet)
        {
            C_Ready readyPacket = packet as C_Ready;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
            if (clientSession.Room == null) return;
            S_Ready ready = new S_Ready { id = clientSession.SessionId };
            clientSession.Room.BroadCast(ready.Write());
        }

        public static void C_GameStartHandler(PacketSession session, IPacket packet)
        {
            C_GameStart gameStart = packet as C_GameStart;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
            if (clientSession.Room == null) return;

            S_GameStart start = new S_GameStart();
            start.users = clientSession.Room.GetAllUserInfo();
        }

        public static void C_GameLoadedHandler(PacketSession session, IPacket packet)
        {
            C_GameLoaded loaded = packet as C_GameLoaded;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
            if (clientSession.Room == null) return;


        }
    }
}
