using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Honeti;

public class WheelShopController : MonoBehaviour
{
    [SerializeField]
    private MeshFilter optionWheel;
    private bool pointed; 
    private Camera cam;
    [SerializeField]
    private TextMesh desc, price, buyPanel, name;

    [SerializeField]
    private I18NTextMesh descI18n, buyPanelI18n, nameI18n;

    [SerializeField]
    private ShopWheel[] options;
    [SerializeField]
    private Transform camPos;
    public int actualOption =0;

    [SerializeField]
    private Outline outlineScript;
    void Start()
    {
        cam = Camera.main;
        actualOption = GlobalVariables.Instance.GetEquippedWheelIndex();
        UpdateActualOption();
    }

    private void UpdateActualOption(){
        if(options[actualOption] != null){
            optionWheel.sharedMesh = options[actualOption].CWheel;
            price.text = "" + options[actualOption].price;
            name.text = "^wheel_name_" + options[actualOption].keyCode;
            desc.text = options[actualOption].price < 0 && !GlobalVariables.Instance.GetBuyStatusWheel(actualOption) ?
             "^wheel_desc_no_price" :
              "^wheel_desc_" + options[actualOption].keyCode;

            if(GlobalVariables.Instance.GetBuyStatusWheel(actualOption))
                buyPanel.text = "^concessionary_equip_panel";
            else 
                buyPanel.text = "^concessionary_buy_panel";
            buyPanelI18n.updateTranslation(true);
            descI18n.updateTranslation(true);
            nameI18n.updateTranslation(true);
        }
    }

    private void Update() {
        if(GlobalVariables.Instance.cameraLookFocusTransform == camPos){            
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? Constants.OUTLINE_WITH_ENABLED : Constants.OUTLINE_WITH_DISABLED);
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))
            || (pointed && Input.GetButtonDown(Constants.BACK))){
                pointed = !pointed;
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed, pointed);
            }
            if(pointed){
                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT))){
                    if(actualOption < options.Length) actualOption++;
                    UpdateActualOption();
                }
                else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))){
                    if(actualOption > 0) actualOption--;
                    UpdateActualOption();
                }

                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE))){
                    if(GlobalVariables.Instance.GetBuyStatusWheel(actualOption)){
                        // wheel is bought
                        GlobalVariables.Instance.EquipWheel(actualOption);
                    }
                    else{
                        // wheel is not bought and we want to buy it
                        if(GlobalVariables.Instance.totalCoins >= options[actualOption].price){
                            GlobalVariables.Instance.totalCoins -= options[actualOption].price;
                            GlobalVariables.Instance.UnlockWheel(actualOption, options[actualOption].price);
                        }
                    }
                }
            }
        }
        //if(pointed) transform.LookAt(cam.transform);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.WHEEL_PANEL_TYPE, camPos, camPos);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.cameraLookFocusTransform == transform){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
