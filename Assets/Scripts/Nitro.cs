using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : InteractableObject
{
    public override void Execute(PlayerController controller)
    {
        controller.AddNitro();
    }
}
