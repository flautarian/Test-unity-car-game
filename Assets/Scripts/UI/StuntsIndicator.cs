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

    private Vector3 initialStuntKeyPos;
    void Start()
    {
        buttonsPressed = new List<StuntIconController>();
        stuntKeysPressed = new List<int>();
        guiController = transform.parent.transform.parent.transform.parent.GetComponent<GUIController>();
        initialStuntKeyPos = new Vector3(-3.6f, 1f, 0);
    }

    internal void startGame(){
        gameStarted = true;
        ReloadStuntList();
    }

    internal void startGameOver(){
        gameStarted = false;
    }

    internal void communicateNewStuntKeyPressed(int keyCode, bool groundedVehicle){
        if(gameStarted && buttonsPressed.Count < Constants.MAX_BUTTONS_STUNTS_COUNT){
            GlobalVariables.Instance.castingStunt = StuntState.CASTING;
            GameObject newKey = GameObject.Instantiate(buttonsAvailable[keyCode]);
            newKey.transform.parent = this.transform;
            Vector3 newPos = initialStuntKeyPos;
            newPos.x += (2f * buttonsPressed.Count);
            newKey.transform.localPosition = newPos;
            var newKeyScript = newKey.GetComponent<StuntIconController>();
            newKeyScript.keyCode = keyCode;
            buttonsPressed.Add(newKeyScript);
            stuntKeysPressed.Add(keyCode);
            var stuntResult = checkStuntList(groundedVehicle);
            if(stuntResult != null){
                communicateStuntCorrect();
                guiController.InitStunt(stuntResult);
            }
        }
        else communicateStuntReset();
    }

    private Stunt checkStuntList(bool groundedVehicle){
        foreach(Stunt st in stuntList){
            if(st != null && st.groundStunt == groundedVehicle && st.compare(stuntKeysPressed))
                return st;
        }
        return null;
    }


    internal void communicateStuntCorrect(){
        GlobalVariables.Instance.castingStunt = StuntState.STUNTCOMPLETED;
        rebootStuntKeys();
    }

    internal void communicateStuntReset(){
        GlobalVariables.Instance.castingStunt = StuntState.STUNTWRONG;
        rebootStuntKeys();
    }
    
    internal void communicateStuntClose(){
        GlobalVariables.Instance.castingStunt = StuntState.OFF;
    }

    internal void rebootStuntKeys(){
        buttonsPressed.Clear();
        stuntKeysPressed.Clear();
    }

    public void UpdateStuntEquippedListAndReload(int index, int newStunt){
        GlobalVariables.Instance.UpdateEquipedScroll(index, newStunt);
        ReloadStuntList();
    }

    private void ReloadStuntList(){
        stuntList = GlobalVariables.Instance.GenerateStuntListWithEquippedStunts();
    }
}
