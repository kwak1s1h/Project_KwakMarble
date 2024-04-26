using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfo : MonoBehaviour
{
    public uint pay;
    public bool isSeleted = false;
    public BuildingType type;
    [SerializeField] private Image chkImg;

    private CreateBuildingUI buildUI;

    private void Start()
    {
        buildUI = UIManager.Instance.GetUI<CreateBuildingUI>();
    }
    private void OnDisable()
    {
        isSeleted = false;
        chkImg.gameObject.SetActive(isSeleted);
    }

    public void Click()
    {
        isSeleted = !isSeleted;
        chkImg.gameObject.SetActive(isSeleted);
        buildUI.ChangeBuildInfo(this);
    }
}
