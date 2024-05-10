using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorUI : UIBase
{
    [SerializeField] private TextMeshProUGUI _errorMsg;

    [SerializeField] private TextMeshProUGUI _submitBtnText;
    [SerializeField] private Button _submitBtn;

    [SerializeField] private TextMeshProUGUI _cancelBtnText;
    [SerializeField] private Button _cancelBtn;

    public string ErrorMessage
    {
        get => _errorMsg.text;
        set => _errorMsg.text = value;
    }

    public string SubmitMessage
    {
        get => _submitBtnText.text;
        set => _submitBtnText.text = value;
    }

    public string CancelMessage
    {
        get => _cancelBtnText.text; 
        set => _cancelBtnText.text = value;
    }

    public void SetSubmitBtnAction(Action action)
    {
        _submitBtn.onClick.AddListener(() => action.Invoke());
    }

    public void SetCancelBtnAction(Action action)
    {
        _cancelBtn.onClick.AddListener(() => action.Invoke());
    }

    public override void TurnOff()
    {

    }

    public override void TurnOn()
    {

    }
}
