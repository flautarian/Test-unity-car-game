using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingObstacle : AccionableObstacle
{
    private void Update()
    {
        if (TriggerController != null && TriggerController.trigger)
        {
            executeAction();
        }
    }

    public override void executeAction() => actionableAnimator.SetBool("action", true);

}
