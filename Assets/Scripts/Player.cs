using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float life;
    public float maxVelocity;
    public float statusCarVelocity;

    GameObject motor;

    void Start()
    {
        motor = GameObject.FindGameObjectWithTag("motorCarreteras");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
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
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            collision.gameObject.GetComponent<Coin>().takeCoin();
        }
    }

private void OnTriggerEnter(Collider collision)
    {
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            if (collision.gameObject.GetComponent<Coin>() != null) collision.gameObject.GetComponent<Coin>().takeCoin();
            else life -= 1;
        }
    }
}
