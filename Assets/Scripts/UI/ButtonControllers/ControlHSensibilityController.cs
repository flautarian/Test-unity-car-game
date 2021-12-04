using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlHSensibilityController : ControlOptionController
{
    private Scrollbar scrollbar;

    private void OnEnable()
    {
        scrollbar = GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((float val) => ScrollbarCallback(val));
        scrollbar.value = GlobalVariables.Instance.GetHSensibilityLevel()*16f;
    }
 
    void ScrollbarCallback(float value)
    {
        GlobalVariables.Instance.UpdateHSensibilityLevel(value/16f);
    }
}
