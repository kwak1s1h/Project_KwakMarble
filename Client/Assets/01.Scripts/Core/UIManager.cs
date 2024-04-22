using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public T GetUI<T>() where T : UIBase
    {
        T ui = FindObjectOfType<T>(true);
        ui.TurnOn();
        return ui;
    }
    public void TurnOffAllUI()
    {
        foreach(var ui in FindObjectsOfType<UIBase>())
        {
            ui.TurnOff();
            ui.gameObject.SetActive(false);
        }
    }
}
