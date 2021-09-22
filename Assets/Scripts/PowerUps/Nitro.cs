using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : InteractableObject
{

    public override void Execute()
    {
        GlobalVariables.Instance.nitroflag = true;
    }
}
