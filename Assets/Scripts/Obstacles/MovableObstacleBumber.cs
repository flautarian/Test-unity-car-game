using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacleBumber : MonoBehaviour
{
    internal bool bumpDetected = false;
    internal float velocityOfFrontVehicle = 0;
    private void OnTriggerEnter(Collider other)
    {
        detectOtherVehicles(other);
    }

    private void OnTriggerStay(Collider other)
    {
        detectOtherVehicles(other);
    }
    private void detectOtherVehicles(Collider other)
    {
        if (Equals(other.gameObject.tag, "StreetObstaculo"))
        {
            bumpDetected = true;
            MovableObstacle otherVehicle = other.GetComponent<MovableObstacle>();
            if (otherVehicle != null)
                velocityOfFrontVehicle = otherVehicle.velocity;
        }
        else bumpDetected = false;
    }

}
