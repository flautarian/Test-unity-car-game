using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController controller;

    public float forwardAccel;
    public float reverseAccel;
    public float turnStrength;
    public float maxWheelTurn;


    void Start()
    {
    }

    internal void executePowerUp(string powerUpButtonName)
    {
        switch (powerUpButtonName)
        {
            case "ThunderButtton":
                break;
            case "StarButtton":
                break;
            case "BulletButtton":
                break;
            default:
                break;
        }
    }

    private void enablePowerUpButton(string v)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Buttons");
        foreach(GameObject button in buttons)
        {
            if (v.Equals(button.name)){
                button.GetComponent<PowerUpButton>().enable();
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(controller != null)
        {
            controller.communicatePlayerBaseCollition(collision);
        }
        
    }

}
