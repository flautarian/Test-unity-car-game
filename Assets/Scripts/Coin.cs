using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableObject
{
    // Start is called before the first frame update

    public Vector3 animPosition;
    public int value;

    void LateUpdate()
    {
        if (!GetComponent<Animator>().GetBool("hasBeenTaken")) return;
        transform.localPosition += animPosition;
    }

    public override void TakeObject(PlayerController controller)
    {
        GetComponent<Animator>().SetBool("hasBeenTaken", true);
        controller.AddCoins(1);
    }
}
