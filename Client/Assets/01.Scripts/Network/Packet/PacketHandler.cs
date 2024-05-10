using DummyClient.Session;
using Server.Packet.Client;
using Server.Packet.Server;
using ServerCore;
using System;
using UnityEngine;

namespace DummyClient
{
    class PacketHandler
    {
        public static void S_SessionInfoHandler(PacketSession session, IPacket packet)
        {
            S_SessionInfo sessionInfo = packet as S_SessionInfo;
            ServerSession serverSession = session as ServerSession;
            Debug.Log(sessionInfo.sessionId);
            serverSession.SessionId = sessionInfo.sessionId;

            C_SetNickname setName = new C_SetNickname();
            setName.value = "asdf";
            session.Send(setName.Write());
        }
    }
}


