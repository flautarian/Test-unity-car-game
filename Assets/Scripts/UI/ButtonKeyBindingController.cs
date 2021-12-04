using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonKeyBindingController : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text up, left, down, right;

    private GameObject currentKey;

    private void Start() {
        keys.Add("Up", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.Instance.GetSavedKeyButton(0)));
        keys.Add("Down", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.Instance.GetSavedKeyButton(1)));
        keys.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.Instance.GetSavedKeyButton(2)));
        keys.Add("Right", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.Instance.GetSavedKeyButton(3)));
        keys.Add("Accelerate", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.Instance.GetSavedKeyButton(4)));
        keys.Add("Stunt", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.Instance.GetSavedKeyButton(5)));

        up.text =  keys["Up"].ToString();
        down.text =  keys["Down"].ToString();
        left.text =  keys["Left"].ToString();
        right.text =  keys["Right"].ToString();
    }

    private void OnGUI() {
        if(currentKey != null) {
            Event e = Event.current;
            if((e.isKey && e.keyCode != KeyCode.Return)){
                keys[currentKey.name] = e.keyCode;
                currentKey.GetComponent<KeyBindingController>().UpdateButton(e.keyCode);
                currentKey = null;
            }
            else if(e.isMouse) {
                keys[currentKey.name] = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Mouse" + e.button);
                currentKey.GetComponent<KeyBindingController>().UpdateButton(keys[currentKey.name]);
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject obj){
        currentKey = obj;
    }
}
