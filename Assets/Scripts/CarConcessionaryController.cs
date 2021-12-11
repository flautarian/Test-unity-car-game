using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Honeti;

public class CarConcessionaryController : MonoBehaviour
{
    [SerializeField]
    private MeshFilter optionCar;
    private bool pointed; 
    private Camera cam;
    [SerializeField]
    private TextMesh stats, price, buyPanel;
    [SerializeField]
    private I18NTextMesh buyPanelI18n;
    [SerializeField]
    private ConcessionaryCar[] options;
    [SerializeField]
    private Transform camPos;
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
            if(GlobalVariables.Instance.GetBuyStatusCar(actualOption))
                buyPanel.text = "^concessionary_equip_panel";
            else 
                buyPanel.text = "^concessionary_buy_panel";
            buyPanelI18n.updateTranslation(true);
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
                    UpdateActualOption();
                }
                else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))){
                    if(actualOption > 0)actualOption--;
                    UpdateActualOption();
                }
                cam.transform.position = camPos.position;
            }
        }
        //if(pointed) transform.LookAt(cam.transform);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.CONCESSIONARY_PANEL_TYPE, this.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.focusTransform == transform){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
