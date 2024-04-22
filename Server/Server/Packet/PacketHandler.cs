using Server.Packet.Client;
using Server.Session;
using ServerCore;
using System;

namespace Server
{
    class PacketHandler 
    {

        public static void C_CreateRoomHandler(PacketSession session, IPacket packet)
        {
            C_CreateRoom movePacket = packet as C_CreateRoom;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
        }

        public static void C_SetNicknameHandler(PacketSession session, IPacket packet)
        {
            C_SetNickname namePacket = packet as C_SetNickname;
            ClientSession clientSession = session as ClientSession;
            if (clientSession == null) return;
            clientSession.Nickname = namePacket.value;
        }
    }
}
