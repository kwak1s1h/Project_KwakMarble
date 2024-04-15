using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Packet
{
    public enum PacketID
    {
        C_CreateRoom,
        S_CreateRoom,

        C_EnterRoom,
        S_EnterRoom,

        C_LeaveRoom,
        S_LeaveRoom,

        S_RemoveRoom,
        S_RoomInfo
    }
}
