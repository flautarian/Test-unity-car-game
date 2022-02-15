using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Honeti;

public class TaxBuildingController : MonoBehaviour
{
    private bool pointed; 
    private Camera cam;

    [SerializeField]
    private TMP_Text levelName;

    [SerializeField]
    private TextMeshProI18n levelNameI18n;

    [SerializeField]
    private LevelSettings[] options;
    [SerializeField]
    private Transform camPos;
    public int actualOption =0, actualLastLevel=0;

    [SerializeField]
    private Outline outlineScript;

    public Player actualCarPlayerController;

    [SerializeField]
    private PlatformController platformController;
    void Start()
    {
        cam = Camera.main;
        actualOption = GlobalVariables.Instance.GetEquippedWheelIndex();
        actualLastLevel = GlobalVariables.Instance.GetActualTaxLastLevel();
        actualOption = actualLastLevel < 0 ? 0 : actualLastLevel;
        UpdateActualOption();
    }

    private void UpdateActualOption(){
        if(options.Length > actualOption && options[actualOption] != null){
            levelName.text = "^level_name_" + actualOption;
            levelNameI18n.UpdateText();
        }
    }

    void Update() {
        if(GlobalVariables.Instance.cameraLookFocusTransform == camPos){            
            if(outlineScript != null)
                outlineScript.updateOutlineLevel(pointed ? Constants.OUTLINE_WITH_ENABLED : Constants.OUTLINE_WITH_DISABLED);
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))
            || (pointed && Input.GetButtonDown(Constants.BACK))){
                pointed = !pointed;
                platformController.MoveToPosition(pointed ? 1 : 0);
                StartCoroutine(ToggleSwitchCam());
            }
            if(actualCarPlayerController != null && (actualCarPlayerController.GetVerticalAxis() == 0f || !actualCarPlayerController.carController.canMove)){
                actualCarPlayerController.transform.rotation = platformController.transform.rotation;
                actualCarPlayerController.transform.position = platformController.transform.position;
            }
            if(pointed){
                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT))){
                    if(actualOption < options.Length && actualOption <= actualLastLevel) actualOption++;
                    UpdateActualOption();
                }
                else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))){
                    if(actualOption > 0) actualOption--;
                    UpdateActualOption();
                }

                if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE))){
                    // TODO: acceder a UI de detalles de nivel
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag.Equals(Constants.GO_TAG_PLAYER) && GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            GlobalVariables.Instance.InvoqueCanvasPanelButton(PanelInteractionType.TAX_TYPE, camPos, camPos);
            if(other.TryGetComponent(out Player p)){
                actualCarPlayerController =  p;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals(Constants.GO_TAG_PLAYER)){
            pointed = false;
            GlobalVariables.Instance.DisableCanvasPanelButton();
            if(platformController.GetActualPosition() > 0)
                platformController.MoveToFirstPosition();
        }
    }

    private IEnumerator ToggleSwitchCam(){
        yield return new WaitForSeconds(0.25f);
        GlobalVariables.Instance.switchCameraFocusToSecondaryObject(pointed, pointed);
        if(!pointed) 
            actualCarPlayerController = null;
    }

}
