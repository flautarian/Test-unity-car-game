using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlChunkController : MonoBehaviour
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
