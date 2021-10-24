using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractionController : MonoBehaviour
{
    public PanelInteractionType panelInteractionType;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HELLO:" + other.tag);
        if (other.tag.Equals(Constants.GO_TAG_PLAYER)){
                GlobalVariables.Instance.InvoqueCanvasPanelButton(panelInteractionType);
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("BYE:" + other.tag);
        if (other.tag.Equals(Constants.GO_TAG_PLAYER)){
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
