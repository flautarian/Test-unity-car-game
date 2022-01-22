using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Honeti;

public class HatShopController : MonoBehaviour
{
    [SerializeField]
    private MeshFilter optionHat;
    private bool pointed; 
    private Camera cam;
    [SerializeField]
    private TextMesh stats, price, buyPanel, name;
    [SerializeField]
    private I18NTextMesh buyPanelI18n, statsI18n, nameI18n;
    [SerializeField]
    private ShopHat[] options;
    [SerializeField]
    private Transform camPos;
    public int actualOption =0;

    [SerializeField]
    private Outline outlineScript;
    void Start()
    {
        cam = Camera.main;
        actualOption = GlobalVariables.Instance.GetEquippedHatIndex();
        UpdateActualOption();
    }

    private void UpdateActualOption(){
        if(options[actualOption] != null){
            optionHat.sharedMesh = options[actualOption].CHat;
            price.text = "" + options[actualOption].price;
            name.text = "^hat_name_" + options[actualOption].keyCode;
            stats.text = "^hat_desc_" + options[actualOption].keyCode;

            if(GlobalVariables.Instance.GetBuyStatusHat(actualOption))
                buyPanel.text = "^concessionary_equip_panel";
            else 
                buyPanel.text = "^concessionary_buy_panel";
            buyPanelI18n.updateTranslation(true);
            statsI18n.updateTranslation(true);
            nameI18n.updateTranslation(true);
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
                    if(actualOption < options.Length) actualOption++;
                    UpdateActualOption();
                }
                else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))){
                    if(actualOption > 0) actualOption--;
                    UpdateActualOption();
                }

                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE))){
                    if(GlobalVariables.Instance.GetBuyStatusHat(actualOption)){
                        // Hat is bought
                        GlobalVariables.Instance.EquipHat(actualOption);
                    }
                    else{
                        // Hat is not bought and we want to buy it
                        if(GlobalVariables.Instance.totalCoins >= options[actualOption].price){
                            GlobalVariables.Instance.totalCoins -= options[actualOption].price;
                            GlobalVariables.Instance.UnlockHat(actualOption, options[actualOption].price);
                        }
                    }
                }
            }
        }
        //if(pointed) transform.LookAt(cam.transform);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.HAT_PANEL_TYPE, this.transform, camPos);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.cameraLookFocusTransform == transform){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
        }
    }
}
