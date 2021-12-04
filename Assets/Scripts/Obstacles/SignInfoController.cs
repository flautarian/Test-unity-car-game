using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInfoController : MonoBehaviour
{
    private Renderer rend;
    private bool pointed; 
    private Camera cam;
    [SerializeField]
    private TextMesh textMesh;
    public Outline outlineScript;
    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
    }

    private void Update() {
        if(GlobalVariables.Instance.focusTransform == transform){            
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? Constants.OUTLINE_WITH_ENABLED : Constants.OUTLINE_WITH_DISABLED);
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))){
                pointed = !pointed;
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed);
            }
        }
        rend.enabled = pointed;
        if(textMesh != null) textMesh.gameObject.SetActive(pointed);
        if(pointed) transform.LookAt(cam.transform);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.INFO_PANEL_TYPE, this.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.focusTransform == transform){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
