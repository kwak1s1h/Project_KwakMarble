using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [SerializeField] private Image checkReady;
    public void SetReady(bool isReady)
    {
        checkReady.gameObject.SetActive(isReady);
    }
}
