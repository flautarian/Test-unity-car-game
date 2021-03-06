using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCameraFocusController : ControlOptionController
{
    private Scrollbar scrollbar;
    private void OnEnable()
    {
        scrollbar = GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((float val) => ScrollbarCallback(val));
        scrollbar.value = (GlobalVariables.Instance.GetFarCameraLevel() -40f) / 50f;
    }

    void ScrollbarCallback(float value)
    {
        GlobalVariables.Instance.UpdateFarCameraLevel(value);
    }
}
