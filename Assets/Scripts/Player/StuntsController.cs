using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntsController : MonoBehaviour
{
    public string[] buttons;

    public float allowedTimeBetweenButtons = 0.3f;
    private float timeLastButtonPressed;

    private bool stuntsModeEnabled = false;

    private bool[] stuntsKeysState = {true, true, true, true}; 

    private int keyPressed = -1;

    public PlayerController playerController;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        if(playerController.canMove && !playerController.turned && GlobalVariables.Instance.inGameState == InGamePanels.GAMEON){
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT)) && !stuntsModeEnabled)
                    ActivateStuntMode();
            else if(stuntsModeEnabled){
                if(Input.GetKeyUp(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT)))
                    DeactivateStuntMode();
                else
                {
                    if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) playerController.communicateStuntReset();
                    if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_UP))) keyPressed= 0;
                    else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN))) keyPressed= 1;
                    else {
                        stuntsKeysState[0] = true;
                        stuntsKeysState[1] = true;
                        keyPressed = -1;
                    }
                    if(keyPressed < 0){
                        if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT))) keyPressed= 2;
                        else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))) keyPressed= 3;
                        else {
                            stuntsKeysState[2] = true;
                            stuntsKeysState[3] = true;
                            keyPressed = -1;
                        }
                    }

                    // invocar un icono de direccion que se ha pulsado para que se vea en pantalla
                    if(keyPressed > -1 && stuntsKeysState[keyPressed]){
                        stuntsKeysState[keyPressed] = false;
                        timeLastButtonPressed = Time.time;
                        playerController.communicateStuntKeyPressed(keyPressed);
                        keyPressed = -1;
                    }
                }
            }
                
        }
        else{
            stuntsModeEnabled = false;
        }
    }

    private void DeactivateStuntMode(){
        playerController.communicateStuntClose();
        stuntsModeEnabled = false;
        playerController.UpdatePlayerAnimationStuntMode(stuntsModeEnabled);
    }

    private void ActivateStuntMode(){
        if(GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            playerController.communicateStuntInitialized();
            stuntsModeEnabled = true;
            playerController.UpdatePlayerAnimationStuntMode(stuntsModeEnabled);
        }
    }
}
