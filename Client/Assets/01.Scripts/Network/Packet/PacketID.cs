namespace Server
{
    public enum PacketID
    {
        C_CreateRoom,
        C_EnterRoom,
        C_SetNickname,
        C_Ready,
        C_GameStart,
        C_GameLoaded,
        C_DrawDice,

        S_SessionInfo,
        S_CreateRoom,
        S_EnterRoom,
        S_EnterRoomRes,
        S_SetTurn,
        S_Ready,
        S_GameStart,
        S_DrawDice,
    }
}