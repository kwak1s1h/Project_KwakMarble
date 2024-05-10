using Server.Packet.Client;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
public class PacketManager
{
    #region 싱글톤
    // 패킷매니저는 수정할 일없으므로 싱글톤으로 간편히 유지
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance{ get { return _instance; } }
    #endregion

    PacketManager() {
        Register();
    }


    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc 
        = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler 
        = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {
        _makeFunc.Add((ushort)PacketID.C_CreateRoom, MakePacket<C_CreateRoom>);
        _handler.Add((ushort)PacketID.C_CreateRoom, PacketHandler.C_CreateRoomHandler);

        _makeFunc.Add((ushort)PacketID.C_EnterRoom, MakePacket<C_EnterRoom>);
        _handler.Add((ushort)PacketID.C_EnterRoom, PacketHandler.C_EnterRoomHandler);

        _makeFunc.Add((ushort)PacketID.C_SetNickname, MakePacket<C_SetNickname>);
        _handler.Add((ushort)PacketID.C_SetNickname, PacketHandler.C_SetNicknameHandler);

        _makeFunc.Add((ushort)PacketID.C_Ready, MakePacket<C_Ready>);
        _handler.Add((ushort)PacketID.C_Ready, PacketHandler.C_ReadyHandler);

        _makeFunc.Add((ushort)PacketID.C_GameStart, MakePacket<C_GameStart>);
        _handler.Add((ushort)PacketID.C_GameStart, PacketHandler.C_GameStartHandler);

        _makeFunc.Add((ushort)PacketID.C_GameLoaded, MakePacket<C_GameLoaded>);
        _handler.Add((ushort)PacketID.C_GameLoaded, PacketHandler.C_GameLoadedHandler);
    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet);
        }
    }

    T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }
}
}
