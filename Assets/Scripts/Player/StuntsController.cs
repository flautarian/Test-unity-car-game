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
        if(Input.GetButtonDown(Constants.INPUT_FIRE) && !stuntsModeEnabled){
            if(!stuntsModeEnabled){
                playerController.communicateStuntInitialized();
                stuntsModeEnabled = true;
                playerController.UpdatePlayerAnimationStuntMode(stuntsModeEnabled);
            }
        }
        else if(stuntsModeEnabled){
            if(Input.GetButtonUp(Constants.INPUT_FIRE))
            {
                playerController.communicateStuntClose();
                stuntsModeEnabled = false;
                playerController.UpdatePlayerAnimationStuntMode(stuntsModeEnabled);
                // TODO: Reiniciar contador de keys pero dejar encendido el modo stunts
            }
            else
            {
                if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) playerController.communicateStuntReset();
                if(Input.GetAxisRaw(Constants.AXIS_VERTICAL) > 0) keyPressed= 0;
                else if(Input.GetAxisRaw(Constants.AXIS_VERTICAL) < 0) keyPressed= 1;
                else {
                    stuntsKeysState[0] = true;
                    stuntsKeysState[1] = true;
                    keyPressed = -1;
                }
                if(keyPressed < 0){
                    if(Input.GetAxisRaw(Constants.AXIS_HORIZONTAL) > 0) keyPressed= 2;
                    else if(Input.GetAxisRaw(Constants.AXIS_HORIZONTAL) < 0) keyPressed= 3;
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
}
