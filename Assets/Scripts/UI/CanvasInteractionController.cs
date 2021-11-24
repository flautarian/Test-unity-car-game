using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractionController : MonoBehaviour
{
    public PanelInteractionType panelInteractionType;

    public Outline outline;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.focusTransform != transform){
                GlobalVariables.Instance.InvoqueCanvasPanelButton(panelInteractionType, transform);
                outline.updateOutlineLevel(Constants.OUTLINE_WITH_ENABLED);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.focusTransform == transform){
            GlobalVariables.Instance.DisableCanvasPanelButton();
            outline.updateOutlineLevel(Constants.OUTLINE_WITH_DISABLED);
        }
    }
}
