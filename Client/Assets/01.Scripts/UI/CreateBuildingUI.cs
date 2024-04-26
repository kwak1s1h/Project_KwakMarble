using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CreateBuildingUI : UIBase
{
    private uint _money = 0;
    public uint Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            buyMoneyTxt.text = value.ToString();
        }
    }

    private CanvasGroup group;
    private const string skipTxt = "스킵";
    [SerializeField] private TextMeshProUGUI buyMoneyTxt;

    [SerializeField] private List<BuildingInfo> infos = new();
    public BuildingType buildingType;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    public void CompleteBuilding()
    {
        //서버에 구매한 내용 보내기
        TurnOff();
        Debug.Log(buildingType);
    }
    public void CloseBuilding()
    {
        //닫기
        TurnOff();
    }

    public void ChangeBuildInfo(BuildingInfo info)
    {
        buildingType ^= info.type;
        if((buildingType & info.type) > 0)
        {
            Money += info.pay;
        }
        else
        {
            Money -= info.pay;
        }
    }

    public override void TurnOn()
    {
        group.alpha = 0;
        transform.localScale = new Vector3(1,1,1);
        group.DOFade(1f, 0.3f);
        buildingType = BuildingType.None;
        buyMoneyTxt.text = skipTxt;
        _money = 0;
        foreach (var item in infos)
        {
            item.isSeleted = false;
        }
    }

    public override void TurnOff()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 1f)).SetEase(Ease.OutExpo);
        seq.Append(transform.DOScale(new Vector3(0f, 0f, 1), 0.3f));
        seq.Join(group.DOFade(0f, 0.3f));
        seq.OnComplete(() => gameObject.SetActive(false));
    }
}
