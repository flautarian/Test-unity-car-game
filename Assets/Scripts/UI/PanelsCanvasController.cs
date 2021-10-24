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

    private PanelInteractionType pit = PanelInteractionType.NO_INTERACTION;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalVariables.Instance.inGameState == InGamePanels.GAMEON){
            if(Input.GetButtonDown(Constants.BACK)){
                GlobalVariables.Instance.inGameState = InGamePanels.PAUSED;
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_PAUSEGAME_PANELS);
            }
            if(pit != PanelInteractionType.NO_INTERACTION &&
             Input.GetButtonDown(Constants.INPUT_FIRE)){
                GlobalVariables.Instance.inGameState = InGamePanels.LEVELSELECTION;
                switch(pit){
                    case PanelInteractionType.TAX_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.MULTIPLAYER_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.LIBRARY_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                    case PanelInteractionType.BRIDGE_TYPE:
                        animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
                    break;
                }
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
                GlobalVariables.Instance.inGameState = InGamePanels.GAMEON;
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_TAX_PANEL_BUTTON);
            }
        }
        if(GlobalVariables.Instance.actualPanelInteractionType != pit){
            pit = GlobalVariables.Instance.actualPanelInteractionType;
            if(pit != PanelInteractionType.NO_INTERACTION)
                panelInteractionAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_PANELBUTTON_ENABLE_INTERACTION);
            else 
                panelInteractionAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_PANELBUTTON_DISABLE_INTERACTION);
            
        }
    }

    public void InvoqueCanvasPanelButton(PanelInteractionType interactionType){
        pit = interactionType;
    }

    public void DisableCanvasPanelButton(){
        pit = PanelInteractionType.NO_INTERACTION;
    }

    public void SetTimeScaleGame(float scale){
        Time.timeScale = scale;
    }

    public void LoadPreviousScene(){
        SceneManager.LoadScene(lastScene);
    }

    public void ReloadActualScene(){
        GlobalVariables.Instance.ResetLevel();
    }

}
