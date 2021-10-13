using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PanelsCanvasController : MonoBehaviour
{
    Animator animator;
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
        }
        else if(GlobalVariables.Instance.inGameState == InGamePanels.PAUSED){
            if(Input.GetButtonDown(Constants.BACK)){
                GlobalVariables.Instance.inGameState = InGamePanels.GAMEON;
                animator.SetTrigger(Constants.ANIMATION_TRIGGER_PAUSEGAME_PANELS);
            }
        }
    }

    public void SetTimeScaleGame(float scale){
        Time.timeScale = scale;
    }

}
