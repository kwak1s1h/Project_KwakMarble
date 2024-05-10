using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [SerializeField] private Image checkReady;
    [SerializeField] private TextMeshProUGUI _idText;
    public void SetReady(bool isReady)
    {
        checkReady.gameObject.SetActive(isReady);
    }

    public void SetId(int id, bool isSelf = false)
    {
        _idText.text = id.ToString();
        if (isSelf) _idText.text += "\nMe";
    }
}
