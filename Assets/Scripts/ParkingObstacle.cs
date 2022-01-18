using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingObstacle : AccionableObstacle
{
    public override void executeAction() => actionableAnimator.SetTrigger("action");
    public override void executeStopAction()
    {
        // nothing yet
    }

}
