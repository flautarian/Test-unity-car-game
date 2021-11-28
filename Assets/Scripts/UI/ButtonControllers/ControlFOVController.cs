using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlFOVController : ControlOptionController
{
    private Scrollbar scrollbar;

    private void OnEnable()
    {
        scrollbar = GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((float val) => ScrollbarCallback(val));
        scrollbar.value = GlobalVariables.Instance.GetFOVLevel();
    }
 
    void ScrollbarCallback(float value)
    {
        GlobalVariables.Instance.UpdateFOVLevel((int) value);
    }
}
