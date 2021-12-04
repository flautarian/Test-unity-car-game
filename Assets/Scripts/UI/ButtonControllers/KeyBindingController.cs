using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingController : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private int key;

    public void UpdateButton(KeyCode keyCode){
        text.text = keyCode.ToString();
        GlobalVariables.Instance.UpdateKeyBinding(key, keyCode);
    }

    private void OnEnable() {
        text.text = GlobalVariables.Instance.GetKeyBindingText(key);
    }
}
