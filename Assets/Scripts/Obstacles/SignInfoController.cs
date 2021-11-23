using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInfoController : MonoBehaviour
{
    private Renderer rend;
    private bool pointed; 
    private Camera cam;
    public Outline outlineScript;
    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
    }

    private void Update() {
        if(GlobalVariables.Instance.focusTransform == transform){
            // outline this
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? 12 : 0);
            if(Input.GetButtonDown(Constants.INPUT_FIRE)){
                pointed = !pointed;
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed);
            }
        }
        if(pointed){
            transform.LookAt(cam.transform);
            rend.enabled = true;
        }
        else {
            rend.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.INFO_PANEL_TYPE, this.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.focusTransform == transform){
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
