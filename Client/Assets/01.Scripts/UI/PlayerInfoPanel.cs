using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoPanel : UIBase
{
    [SerializeField] List<PlayerPanel> moneyText = new();
    private Dictionary<int, PlayerPanel> uuidByMoney;

    public void ChangeMoney(int uuid, int money)
    {
        uuidByMoney[uuid].SetText(money);
    }
    public void Init(int[] uuids)
    {
        for (int i = 0; i < uuids.Length; i++)
        {
            moneyText[i].SetText(1000000);
            moneyText[i].gameObject.SetActive(true);
            uuidByMoney.Add(uuids[i],moneyText[i]);
        }
    }

    public override void TurnOn()
    {
    }

    public override void TurnOff()
    {
    }
}
