using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Place : MonoBehaviour
{
    public Transform placeTrm;
    private Sequence seq = null;
    private float yPos;
    public void WavePlace()
    {
        if (seq == null)
        {
            MkSeq();
            return;
        }

            if (seq.IsComplete())
        {
            seq.Kill();
            Vector3 pos = transform.position;
            pos.y = yPos;
            transform.position = pos;
        }
        MkSeq();
    }
    private void MkSeq()
    {
        seq = DOTween.Sequence();
        float yPos = transform.position.y;
        seq.Append(transform.DOMoveY(transform.position.y - 5, 0.1f));
        seq.Append(transform.DOMoveY(yPos, 0.3f)).SetEase(Ease.OutElastic);
    }
}
