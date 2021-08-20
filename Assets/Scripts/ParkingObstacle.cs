using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingObstacle : AccionableObstacle
{
    private void Update()
    {
        if(TriggerController != null && TriggerController.trigger)
        {
            executeAction();
        }
    }
    public override void Collide(Transform c)
    {
        // non necessary at this point
    }

    public override void executeAction() => actionableAnimator.SetBool("action", true);

    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
        // non necessary at this point
    }
}
