using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{
    public override void TakeObject(PlayerController controller)
    {
        GetComponent<Animator>().SetBool("hasBeenTaken", true);
        controller.RecoverParts();
    }
}
