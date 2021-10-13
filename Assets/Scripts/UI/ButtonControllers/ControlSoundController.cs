using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSoundController : MonoBehaviour
{
    private Scrollbar scrollbar;

    private void OnEnable()
    {
        scrollbar = GetComponent<Scrollbar>();
        scrollbar.onValueChanged.AddListener((float val) => ScrollbarCallback(val));
        scrollbar.value = GlobalVariables.Instance.GetSoundLevel();
    }
 
    void ScrollbarCallback(float value)
    {
        GlobalVariables.Instance.UpdateSoundLevel(value);
    }
}
