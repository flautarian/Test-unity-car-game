using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntsIndicator : MonoBehaviour
{
    // Start is called before the first frame update

    bool gameStarted = false;

    public GameObject[] buttonsAvailable;
    public List<StuntIconController> buttonsPressed;
    void Start()
    {
        buttonsPressed = new List<StuntIconController>();
    }

    internal void startGame(){
        gameStarted = true;
    }

    internal void startGameOver(){
        gameStarted = false;
    }

    internal void communicateNewStuntKeyPressed(int keyCode){
        if(gameStarted){
            GameObject newKey = GameObject.Instantiate(buttonsAvailable[keyCode]);
            newKey.transform.parent = this.transform;
            newKey.transform.localPosition =  Vector3.right * (0.2f * buttonsPressed.Count);
            StuntIconController stuntIcon = newKey.GetComponent<StuntIconController>();
            buttonsPressed.Add(stuntIcon);
        }
    }

    internal void communicateStuntInitialized(){
        if(gameStarted){
            //TODO: lanzar animacion de comienzo de trackeo de stunt keys
        }
    }

    internal void communicateStuntReset(){
        //buttonsPressed.Clear();
    }
}
