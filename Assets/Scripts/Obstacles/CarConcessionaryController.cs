using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarConcessionaryController : MonoBehaviour
{
    private MeshFilter optionCar;
    private bool pointed; 
    private Camera cam;
    [SerializeField]
    private TextMesh stats, price;
    [SerializeField]
    private ConcessionaryCar[] options;
    public int actualOption =0;
    public Outline outlineScript;
    void Start()
    {
        cam = Camera.main;
        UpdateActualOption();
    }

    private void UpdateActualOption(){
        if(options[actualOption] != null){
            optionCar.sharedMesh = options[actualOption].CCar;
            stats.text = "\n" + options[actualOption].playerInfoClass.forwardAccel
            + "\n" + options[actualOption].playerInfoClass.turnStrength
            + "\n" + options[actualOption].playerInfoClass.gravityForce
            + "\n" + options[actualOption].playerInfoClass.dragGroundForce;
            price.text = "" + options[actualOption].price;
        }
    }

    private void Update() {
        if(GlobalVariables.Instance.focusTransform == transform){            
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? Constants.OUTLINE_WITH_ENABLED : Constants.OUTLINE_WITH_DISABLED);
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))
            || (pointed && Input.GetButtonDown(Constants.BACK))){
                pointed = !pointed;
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed);
            }
            if(pointed){
                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT))){
                    if(actualOption < options.Length)actualOption++;
                }
                else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))){
                    if(actualOption > 0)actualOption--;
                }
            }
        }
        //if(pointed) transform.LookAt(cam.transform);
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
