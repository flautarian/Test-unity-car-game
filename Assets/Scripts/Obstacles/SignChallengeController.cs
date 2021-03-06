using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignChallengeController : MonoBehaviour
{
    private bool pointed; 
    private Camera cam;
    public Outline outlineScript;
    private LevelSettings lvlChallenge;
    void Start()
    {
        cam = Camera.main;
        lvlChallenge = GetComponent<LevelSettings>();
    }

    private void Update() {
        if(GlobalVariables.Instance.cameraLookFocusTransform == transform){            
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? Constants.OUTLINE_WITH_ENABLED : Constants.OUTLINE_WITH_DISABLED);

            if(!GlobalVariables.Instance.IsPlayerRunning() &&
                (Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))
                || (pointed && Input.GetButtonDown(Constants.BACK)))){
                pointed = !pointed;
                ManageSignChallengeChange();
            }
        }
        
    }

    private void ManageSignChallengeChange(){
        if(pointed) {
            GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_OK_UI_BUTTON, 1f);
            //GlobalVariables.Instance.PrepareGlobalToLevel(lvlChallenge);
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.CHALLENGE_TYPE, this.transform, this.transform);
            transform.LookAt(cam.transform);
        }
        else
            GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_BACK_UI_BUTTON, 1f);
        GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed, false);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.CHALLENGE_TYPE, this.transform, null);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.cameraLookFocusTransform == transform){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
