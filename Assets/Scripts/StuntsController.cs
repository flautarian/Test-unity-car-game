using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntsController : MonoBehaviour
{
    public string[] buttons;
    private int currentIndex = 0;

    public float allowedTimeBetweenButtons = 0.3f;
    private float timeLastButtonPressed;

    private bool stuntsModeEnabled = false;

    private bool[] stuntsKeysState = {true, true, true, true}; 

    private int keyPressed = -1;

    public GameObject[] buttonsAvailable;

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
            }
        }
        else if(stuntsModeEnabled){

            if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) playerController.communicateStuntReset();
            
            if(Input.GetAxis(Constants.AXIS_VERTICAL) > 0) {keyPressed= 0;}
            else if(Input.GetAxis(Constants.AXIS_VERTICAL) < 0) {keyPressed= 1;}
            else {
                stuntsKeysState[0] = true;
                stuntsKeysState[1] = true;
            }

            if(Input.GetAxis(Constants.AXIS_HORIZONTAL) > 0) {keyPressed= 2;}
            else if(Input.GetAxis(Constants.AXIS_HORIZONTAL) < 0) {keyPressed= 3;}
            else {
                stuntsKeysState[2] = true;
                stuntsKeysState[3] = true;
            }

            if(keyPressed > -1 && stuntsKeysState[keyPressed]){
                // invocar un icono de direccion que se ha pulsado para que se vea en pantalla
                Debug.Log("incluyendo tecla " + keyPressed + " en el stunt");
                stuntsKeysState[keyPressed] = false;
                timeLastButtonPressed = Time.time;
                playerController.communicateStuntKeyPressed(keyPressed);
                keyPressed = -1;
            }
            // TODO: Reiniciar contador de keys pero dejar encendido el modo stunts
            playerController.communicateStuntReset();
        }
    }
}
