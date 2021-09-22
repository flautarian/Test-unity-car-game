using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{

    public override void Execute()
    {
        GlobalVariables.Instance.repairflag = true;
    }
}
