using Server.Packet.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterUI : UIBase
{
    public void CreateRoom()
    {
        //�� �游��ž�
        C_CreateRoom packet = new C_CreateRoom();
        NetworkManager.Instance.Send(packet.Write());
    }
    public void EnterRoom()
    {
        UIManager.Instance.GetUI<RoomIdUI>();
    }
    public override void TurnOff()
    {
    }

    public override void TurnOn()
    {
    }
}
