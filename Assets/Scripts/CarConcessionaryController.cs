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

    [SerializeField]
    private Outline outlineScript;

    [SerializeField]
    private BarController velBar, strngBar, gravBar, dragBar, stuntBar, accelBar;
    void Start()
    {
        cam = Camera.main;
        UpdateActualOption();
    }

    private void UpdateActualOption(){
        if(options[actualOption] != null){
            optionCar.sharedMesh = options[actualOption].CCar;
            price.text = "" + options[actualOption].price;

            velBar.UpdateValue((int)((options[actualOption].playerInfoClass.forwardAccel * 100) / Constants.MAX_VELOCITY_CARS));
            strngBar.UpdateValue((int)((options[actualOption].playerInfoClass.turnStrength * 100) / Constants.MAX_TURN_STRENGTH_CARS));
            gravBar.UpdateValue((int)((options[actualOption].playerInfoClass.gravityForce * 100) / Constants.MAX_GRAVITY_FORCE_CARS));
            dragBar.UpdateValue((int)((options[actualOption].playerInfoClass.dragGroundForce * 100) / Constants.MAX_DRAG_FORCE_CARS));
            stuntBar.UpdateValue((int)((options[actualOption].playerInfoClass.stuntHability * 100) / Constants.MAX_STUNT_HABILITY_CARS));
            accelBar.UpdateValue((int)((options[actualOption].playerInfoClass.accel * 100) / Constants.MAX_ACCEL_CARS));

            if(GlobalVariables.Instance.GetBuyStatusCar(actualOption))
                buyPanel.text = "^concessionary_equip_panel";
            else 
                buyPanel.text = "^concessionary_buy_panel";
            buyPanelI18n.updateTranslation(true);
        }
    }

    private void Update() {
        if(GlobalVariables.Instance.cameraLookFocusTransform == transform){            
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? Constants.OUTLINE_WITH_ENABLED : Constants.OUTLINE_WITH_DISABLED);
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))
            || (pointed && Input.GetButtonDown(Constants.BACK))){
                pointed = !pointed;
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed, pointed);
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

                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE))){
                    if(GlobalVariables.Instance.GetBuyStatusCar(actualOption)){
                        // car is bought
                        GlobalVariables.Instance.EquipCar(actualOption);
                    }
                    else{
                        // car is not bought and we want to buy it
                        if(GlobalVariables.Instance.totalCoins >= options[actualOption].price){
                            GlobalVariables.Instance.totalCoins -= options[actualOption].price;
                            GlobalVariables.Instance.UnlockCar(actualOption, options[actualOption].price);
                        }
                    }
                }
            }
        }
        //if(pointed) transform.LookAt(cam.transform);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.CONCESSIONARY_PANEL_TYPE, this.transform, camPos);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.cameraLookFocusTransform == transform){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
