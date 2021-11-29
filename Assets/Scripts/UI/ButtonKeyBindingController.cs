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
        //keys.Add("Up", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.GetSavedKeyButton("Up")));
        //keys.Add("Down", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.GetSavedKeyButton("Down")));
        //keys.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.GetSavedKeyButton("Left")));
        //keys.Add("Right", (KeyCode) System.Enum.Parse(typeof(KeyCode), GlobalVariables.GetSavedKeyButton("Right")));

        up.text =  keys["Up"].ToString();
        down.text =  keys["Down"].ToString();
        left.text =  keys["Left"].ToString();
        right.text =  keys["Right"].ToString();
    }

    private void OnGUI() {
        if(currentKey != null){
            Event e = Event.current;
            if(e.isKey){
                keys[currentKey.name] = e.keyCode;
                currentKey.GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject obj){
        currentKey = obj;
    }
}
