using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntsIndicator : MonoBehaviour
{
    // Start is called before the first frame update

    bool gameStarted = false;

    public GameObject[] buttonsAvailable;

    private GUIController guiController;
    public List<StuntIconController> buttonsPressed;

    public Stunt[] stuntList;

    public List<int> stuntKeysPressed;
    void Start()
    {
        buttonsPressed = new List<StuntIconController>();
        stuntKeysPressed = new List<int>();
        guiController = transform.parent.transform.parent.GetComponent<GUIController>();
    }

    internal void startGame(){
        gameStarted = true;
    }

    internal void startGameOver(){
        gameStarted = false;
    }

    internal void communicateNewStuntKeyPressed(int keyCode, bool groundedVehicle){
        if(gameStarted && buttonsPressed.Count < Constants.MAX_BUTTONS_STUNTS_COUNT){
            GlobalVariables.Instance.castingStunt = StuntState.CASTING;
            GameObject newKey = GameObject.Instantiate(buttonsAvailable[keyCode]);
            newKey.transform.parent = this.transform;
            Vector3 newPos = Vector3.zero;
            newPos.x = -3.6f + (2f * buttonsPressed.Count);
            newPos.y = 1f;
            newKey.transform.localPosition = newPos;
            var newKeyScript = newKey.GetComponent<StuntIconController>();
            newKeyScript.keyCode = keyCode;
            buttonsPressed.Add(newKeyScript);
            stuntKeysPressed.Add(keyCode);
            var stuntResult = checkStuntList(groundedVehicle);
            if(stuntResult > -1){
                communicateStuntCorrect(stuntResult);
                guiController.InitStunt(stuntResult);
            }
        }
        else communicateStuntReset();
    }

    private int checkStuntList(bool groundedVehicle){
        foreach(Stunt st in stuntList){
            if(st.groundStunt == groundedVehicle && st.compare(stuntKeysPressed)){
                Debug.Log("Casting " + st.stuntName + "!!");
                return st.stuntValue;
            }
        }
        return -1;
    }

    internal void communicateStuntInitialized(){
        if(gameStarted){
            //TODO: lanzar animacion de comienzo de trackeo de stunt keys
        }
    }

    internal void communicateStuntCorrect(int stunt){
        GlobalVariables.Instance.castingStunt = StuntState.STUNTCOMPLETED;
        buttonsPressed.Clear();
        stuntKeysPressed.Clear();
    }

    internal void communicateStuntReset(){
        GlobalVariables.Instance.castingStunt = StuntState.STUNTWRONG;
        buttonsPressed.Clear();
        stuntKeysPressed.Clear();
    }
    
    internal void communicateStuntClose(){
        GlobalVariables.Instance.castingStunt = StuntState.OFF;
        buttonsPressed.Clear();
        stuntKeysPressed.Clear();
    }
}
