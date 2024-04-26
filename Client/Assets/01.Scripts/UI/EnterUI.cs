using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterUI : UIBase
{
    public void CreateRoom()
    {
        //나 방만들거야
        UIManager.Instance.GetUI<RoomUI>();
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
