using Network;
using Network.Packet;
using Network.Packet.Server;
using System;
using System.Collections.Generic;

namespace Server
{
    public class PacketManager
    {
        #region 싱글톤
        // 패킷매니저는 수정할 일없으므로 싱글톤으로 간편히 유지
        static PacketManager _instance = new PacketManager();
        public static PacketManager Instance { get { return _instance; } }
        #endregion

        PacketManager()
        {
            Register();
        }


        Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc
            = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
        Dictionary<ushort, Action<PacketSession, IPacket>> _handler
            = new Dictionary<ushort, Action<PacketSession, IPacket>>();

        public void Register()
        {
            _makeFunc.Add((ushort)PacketID.S_EnterRoom, MakePacket<S_EnterRoom>);
            _handler.Add((ushort)PacketID.S_EnterRoom, PacketHandler.S_EnterRoomHandler);

            _makeFunc.Add((ushort)PacketID.S_LeaveRoom, MakePacket<S_LeaveRoom>);
            _handler.Add((ushort)PacketID.S_LeaveRoom, PacketHandler.S_LeaveRoomHandler);

            _makeFunc.Add((ushort)PacketID.S_RoomInfo, MakePacket<S_RoomInfo>);
            _handler.Add((ushort)PacketID.S_RoomInfo, PacketHandler.S_RoomInfoHandler);
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
