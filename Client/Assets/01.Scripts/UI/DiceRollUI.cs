using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Server.Packet.Client;

public class DiceRollUI : UIBase
{
    private Sequence seq;
    [SerializeField] private TextMeshProUGUI rollText;
    public void RollDice()
    {
        //int fir = Random.Range(1, 7);
        //int sec = Random.Range(1, 7);
        //print($"{fir }/{sec}");
        //Board.Instance.RollDice(new int[2] { fir, sec });
        C_DrawDice dice = new C_DrawDice();
        NetworkManager.Instance.Send(dice.Write());
    }

    public override void TurnOff()
    {
        seq.Kill();
    }

    public override void TurnOn()
    {
        seq = DOTween.Sequence();
        seq.Append(rollText.transform.DOLocalMoveY(110, 1.2f)).SetLoops(-1, LoopType.Yoyo);
    }
}
