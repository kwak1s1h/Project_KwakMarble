using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomIdUI : UIBase
{
    [SerializeField] private TMP_InputField roomIdInput;
    public void EnterRoom()
    {
        //서버에 보냄 나 방에 들어감!
    }
    public override void TurnOff()
    {
    }
    public override void TurnOn()
    {
    }
}
