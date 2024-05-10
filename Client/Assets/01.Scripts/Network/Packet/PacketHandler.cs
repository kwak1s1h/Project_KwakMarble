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

        public static void S_CreateRoomHandler(PacketSession session, IPacket packet)
        {
            S_CreateRoom res = packet as S_CreateRoom;
            ServerSession serverSession = session as ServerSession;
            RoomUI ui = UIManager.Instance.GetUI<RoomUI>();
            ui.SetRoomUuid(res.roomCode);
            ui.Init(serverSession.SessionId);
        }

        public static void S_EnterRoomHandler(PacketSession session, IPacket packet)
        {
            S_EnterRoom enter = packet as S_EnterRoom;
            RoomUI roomUI = UIManager.Instance.GetUI<RoomUI>();
            roomUI.OtherEnterRoom(enter.id);
        }

        public static void S_EnterRoomResHandler(PacketSession session, IPacket packet)
        {
            S_EnterRoomRes res = packet as S_EnterRoomRes;
            ServerSession serverSession = session as ServerSession;

            UIManager.Instance.TurnOffAllUI();
            if (!res.success)
            {
                // 실패 안내
                UIManager.Instance.GetUI<EnterUI>();
                return;
            }
            RoomUI roomUI = UIManager.Instance.GetUI<RoomUI>();
            roomUI.SetRoomUuid(res.roomId);
            roomUI.Init(serverSession.SessionId);
            foreach(int id in res.users)
            {
                if(serverSession.SessionId != id)
                    roomUI.OtherEnterRoom(id);
            }
        }

        public static void S_ReadyHandler(PacketSession session, IPacket packet)
        {
            S_Ready ready = packet as S_Ready;
            ServerSession serverSession = session as ServerSession;
            if (serverSession == null) return;
            RoomUI roomUI = UIManager.Instance.GetUI<RoomUI>();
            roomUI.ChangeReady(ready.id, ready.value);
        }

        public static void S_GameStartHandler(PacketSession session, IPacket packet)
        {
            S_GameStart start = packet as S_GameStart;
            ServerSession serverSession = session as ServerSession;
            RoomUI roomUI = UIManager.Instance.GetUI<RoomUI>();
            roomUI.GameStart(start.users);
        }

        public static void S_SetTurnHandler(PacketSession session, IPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}


