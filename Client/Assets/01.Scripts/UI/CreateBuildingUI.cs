using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CreateBuildingUI : UIBase
{
    private CanvasGroup group;
    private uint money = 20000;
    [SerializeField] private TextMeshProUGUI buyMoneyTxt;

    public BuildingType buildingType;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    public void CompleteBuilding()
    {
        //서버에 구매한 내용 보내기
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1),1f)).SetEase(Ease.OutExpo);
        seq.Append(transform.DOScale(new Vector3(0f, 0f, 1),0.3f));
        seq.Join(group.DOFade(0f,0.3f));
        seq.OnComplete(() => gameObject.SetActive(false));
    }
    public void CloseBuilding()
    {
        //닫기
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 1f)).SetEase(Ease.OutExpo);
        seq.Append(transform.DOScale(new Vector3(0f, 0f, 1), 0.3f));
        seq.Join(group.DOFade(0f, 0.3f));
        seq.OnComplete(() => gameObject.SetActive(false));
    }

    public override void TurnOn()
    {
        buildingType = BuildingType.None;
        money = 20000;
        buyMoneyTxt.text = money.ToString();
    }

    public override void TurnOff()
    {
    }
}
