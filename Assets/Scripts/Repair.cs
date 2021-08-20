using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{
    public override void Execute(PlayerController controller)
    {
        controller.RecoverParts();
    }
}
