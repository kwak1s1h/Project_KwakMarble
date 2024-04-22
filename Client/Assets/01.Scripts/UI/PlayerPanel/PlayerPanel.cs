using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI moneyTxt;

    public void SetText(int money) => moneyTxt.text = money.ToString();
}
