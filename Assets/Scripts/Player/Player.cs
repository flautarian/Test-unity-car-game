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
    public float gravityForce;
    public float dragGroundForce;

    public List<PlayerDestructablePart> parts;

    public void OnCollisionEnter(Collision collision)
    {
        if(controller != null)
        {
            controller.communicatePlayerBaseCollition(collision);
        }
        
    }

}
