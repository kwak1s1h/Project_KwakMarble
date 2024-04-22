using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DiceResult : Image
{
    private TextMeshProUGUI resultText;

    protected override void Awake()
    {
        base.Awake();
        resultText = transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
    }

    public void SetText(int result)
    {
        resultText.text = result.ToString();
        StartCoroutine(DeleteResult());
    }
    private IEnumerator DeleteResult()
    {
        yield return new WaitForSeconds(5f);
        Sequence seq = DOTween.Sequence();
        seq.Append(this.DOFade(0f,1f));
        seq.Join(transform.DOScale(0, 1f));
    }
}
