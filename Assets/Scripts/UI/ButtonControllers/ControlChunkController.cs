using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlChunkController : ControlOptionController
{
    private Scrollbar scrollbar;
    private void OnEnable()
    {
        scrollbar = GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((float val) => ScrollbarCallback(val));
        scrollbar.value = GlobalVariables.Instance.GetChunkLevel();
    }
    
    void ScrollbarCallback(float value)
    {
        GlobalVariables.Instance.UpdateChunkLevel(value);
    }
}
