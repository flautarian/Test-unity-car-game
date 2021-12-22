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

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        // Control menus accessibles desde mon obert
        if(GlobalVariables.Instance.inGameState == InGamePanels.GAMEON && 
        GlobalVariables.Instance.actualPanelInteractionType != PanelInteractionType.INFO_PANEL_TYPE &&
        GlobalVariables.Instance.actualPanelInteractionType != PanelInteractionType.CONCESSIONARY_PANEL_TYPE){
            if(Input.GetButtonDown(Constants.BACK)){
                GlobalVariables.Instance.inGameState = InGamePanels.PAUSED;
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_PAUSEGAME_PANELS);
            }
            if(panelInteractionType != PanelInteractionType.NO_INTERACTION &&
             Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT))){
                GlobalVariables.Instance.inGameState = InGamePanels.LEVELSELECTION;
                switch(panelInteractionType){
                    case PanelInteractionType.TAX_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.MULTIPLAYER_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.LIBRARY_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_LIBRARY_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.BRIDGE_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                }
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(true, false);
            }
        }
        else if(GlobalVariables.Instance.inGameState == InGamePanels.PAUSED){
            if(Input.GetButtonDown(Constants.BACK)){
                GlobalVariables.Instance.inGameState = InGamePanels.GAMEON;
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_PAUSEGAME_PANELS);
            }
        }
        else if(GlobalVariables.Instance.inGameState == InGamePanels.LEVELSELECTION){
            if(Input.GetButtonDown(Constants.BACK)){
                GlobalVariables.Instance.switchCameraFocusToSecondaryObject(false, false);
                GlobalVariables.Instance.inGameState = InGamePanels.GAMEON;
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_GO_BACK_PANEL_BUTTON);
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

    public void InvoqueCanvasPanelButton(PanelInteractionType interactionType){
        panelInteractionType = interactionType;
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
