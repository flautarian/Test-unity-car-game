using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelsCanvasController : MonoBehaviour
{
    Animator animator;

    public Animator panelInteractionAnimator;

    public int lastScene;

    private PanelInteractionType panelInteractionType = PanelInteractionType.NO_INTERACTION;

    public TurnedUpController turnedUpController;

    [SerializeField]
    private PanelLibraryCanvasController panelLibraryCanvasController;
    [SerializeField]
    private PanelLibraryEquipCanvasController panelLibraryEquipCanvasController;
    [SerializeField]
    private PanelTaxCanvasController panelTaxCanvasController;
    [SerializeField]
    private PanelGameWonController panelGameWonController;
    [SerializeField]
    private PanelGameLostController panelGameLostController;
    [SerializeField]
    private LvlDetailsPanelController lvlDetailsPanelController;

    void Start()
    {
        animator = GetComponent<Animator>();
        // code to clean all UI options that will not be used in the actual scene
        panelLibraryCanvasController.ExistenceCheck();
        panelLibraryEquipCanvasController.ExistenceCheck();
        panelTaxCanvasController.ExistenceCheck();
        panelGameWonController.ExistenceCheck();
        panelGameLostController.ExistenceCheck();
        lvlDetailsPanelController.ExistenceCheck();
    }


    void Update()
    {
        // Control menus accessibles desde mon obert
        if(IsGameOnOrPaused() || IsInExternalUIState()){
            if(Input.GetButtonDown(Constants.BACK) && GlobalVariables.Instance.playerTargetedByCamera){
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_PAUSEGAME_PANELS);
            }
            else if(panelInteractionType != PanelInteractionType.NO_INTERACTION &&
             Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))){
                switch(panelInteractionType){
                    case PanelInteractionType.TAX_TYPE:
                        ExecuteAnimationToActiveUI(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.MULTIPLAYER_TYPE:
                        ExecuteAnimationToActiveUI(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.LIBRARY_TYPE:
                        ExecuteAnimationToActiveUI(Constants.ANIMATION_TRIGGER_LIBRARY_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.BRIDGE_TYPE:
                        ExecuteAnimationToActiveUI(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.CHALLENGE_TYPE:
                        ExecuteAnimationToActiveUI(Constants.ANIMATION_TRIGGER_CHALLENGE_INTERACTION);
                    break;
                    case PanelInteractionType.RELICS_TYPE:
                        ExecuteAnimationToActiveUI(Constants.ANIMATION_TRIGGER_RELIC_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.CONCESSIONARY_PANEL_TYPE:
                    case PanelInteractionType.INFO_PANEL_TYPE:
                        // code implemented in other site
                    break;
                }
            }
        }
        else if(GlobalVariables.Instance.inGameState == InGamePanels.LEVELSELECTION){
            if(Input.GetButtonDown(Constants.BACK)){
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(false, false);
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_GO_BACK_PANEL_BUTTON);
                GlobalVariables.Instance.inGameState = InGamePanels.GAMEON;
            }
        }
        else if(GlobalVariables.Instance.inGameState == InGamePanels.SUBUI1){
            if(Input.GetButtonDown(Constants.BACK)){
                ExecuteSubUI1Trigger();
            }
        }

        //Control acces a menus desde mon obert
        if(GlobalVariables.Instance.actualPanelInteractionType != panelInteractionType){
            panelInteractionType = GlobalVariables.Instance.actualPanelInteractionType;
        }
        //Control de sistema de cotxe turned up
        if(turnedUpController.gameObject.activeSelf) {
            if(!GlobalVariables.Instance.turnedCar)
                turnedUpController.gameObject.SetActive(false);
        }
        else if(GlobalVariables.Instance.turnedCar)
            turnedUpController.gameObject.SetActive(true);
    }

    public void ExecuteSubUI1Trigger(){
        switch(panelInteractionType){
            case PanelInteractionType.TAX_TYPE:
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
            break;
            case PanelInteractionType.MULTIPLAYER_TYPE:
                // Nothing here
            break;
            case PanelInteractionType.LIBRARY_TYPE:
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_LIBRARY_INTERACTION);
            break;
            case PanelInteractionType.BRIDGE_TYPE:
                // Nothing here
            break;
            case PanelInteractionType.CHALLENGE_TYPE:
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_CHALLENGE_INTERACTION);
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(false, false);
            break;
            case PanelInteractionType.CONCESSIONARY_PANEL_TYPE:
            case PanelInteractionType.INFO_PANEL_TYPE:
                // code implemented in other site
            break;
        }
    }

    public void UpdateGameState( InGamePanels state){
        GlobalVariables.Instance.inGameState = state;
    }

    public void PlayChunkFromGlobalVariables(string chunk){
        GlobalVariables.Instance.GetAndPlayChunk(chunk, 1f);
    }

    public void InvoqueCanvasPanelButton(PanelInteractionType interactionType){
        panelInteractionType = interactionType;
    }

    private void ExecuteAnimationToActiveUI(string Interaction){
        animator.SetTrigger(Interaction);
        GlobalVariables.Instance.switchCameraFocusToSecondaryObject(true, false);
    }

    private bool IsGameOnOrPaused(){
        return GlobalVariables.Instance.inGameState == InGamePanels.GAMEON || 
        GlobalVariables.Instance.inGameState == InGamePanels.PAUSED;
    }

    private bool IsInExternalUIState(){
        return  GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.INFO_PANEL_TYPE ||
        GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.CONCESSIONARY_PANEL_TYPE;
    }

    public void DisableCanvasPanelButton(){
        panelInteractionType = PanelInteractionType.NO_INTERACTION;
    }

    public void SetTimeScaleGame(float scale){
        Time.timeScale = scale;
    }

    public void LoadPreviousScene(){
        switch(lastScene){
            // WORLD
            case 1:
                GlobalVariables.Instance.gameMode = GameMode.WOLRDMAINMENU;
            break;
            // MAIN MENU
            case 0:
                GlobalVariables.Instance.gameMode = GameMode.MAINMENU;
            break;
            default:
                GlobalVariables.Instance.gameMode = GameMode.WOLRDMAINMENU;
            break;
        };
        GlobalVariables.Instance.actualPanelInteractionType = PanelInteractionType.NO_INTERACTION;
        SceneManager.LoadScene(lastScene);
    }

    public void ReloadActualScene(){
        GlobalVariables.Instance.ResetLevel();
    }

    public void GoToTaxLevel(){
        GlobalVariables.Instance.GoToTaxLevel();
    }

}
