using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DiceRollUI : UIBase
{
    private Sequence seq;
    [SerializeField] private TextMeshProUGUI rollText;
    public void RollDice()
    {
        int fir = Random.Range(1, 7);
        int sec = Random.Range(1, 7);
        print($"{fir}/{sec}");
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
